﻿using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIUser.Controllers
{
   // [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class FeedBackController : ControllerBase
    {
        IRepository<Comment> CommentRepo;
        IRepository<Rates> RateRepo;
        IUnitOfWork IunitOfWork;
        ResultViewModel Result;

        public FeedBackController(IUnitOfWork _IunitOfWork)
        {
            IunitOfWork = _IunitOfWork;
            CommentRepo = IunitOfWork.GetCommentRepo();
            RateRepo = IunitOfWork.GetRateRepo();
            Result = new ResultViewModel();
        }


        //return  All Comments by ProductID 
        [HttpGet]
        [Route("Comments/{PrdID}")]
        public async Task<ResultViewModel> GetAllComment(int PrdID)
        {
            Result.Data = await CommentRepo.FindByCondition(i => i.ProductId == PrdID);
            Result.IsSucess = true;
            return Result;
        }

        // Add Comment
        [HttpPost]
        [Route("Comments")]
        public async Task<ResultViewModel> AddComment( Comment comment)
        {
            Result.Data = await CommentRepo.Add(comment);
            await IunitOfWork.Save();
            Result.Message = "Add Successfully";
            Result.IsSucess = true;
            return Result;
        }

        // Delete  Comment
        [HttpDelete]
        [Route("Comments")]
        public async Task<ResultViewModel> DeleteComment(Comment comment)
        {
            Result.Data = await CommentRepo.Remove(comment);
            await IunitOfWork.Save();
            Result.Message = "Deleted Successfully";
            Result.IsSucess = true;
            return Result;
        }


        // Add Rate
        [HttpPost]
        [Route("Rate/add")]
        public async Task<ResultViewModel> AddRate(Rates rate)
        {
            Result.Data = await RateRepo.Add(rate);
            Result.Message = "Add Successfully";
            await IunitOfWork.Save();
            Result.IsSucess = true;
            return Result;
        }

        // Get Rate By Product ID 
        [HttpGet]
        [Route("Rate/{PrdID}")]
        public async Task<ResultViewModel> GetRate(int  PrdID)
        {

            int Rate = 0;
            int finalRate = 0;
            var Rates = await RateRepo.FindByCondition(i => i.ProductId == PrdID) as IEnumerable<Rates>; 
           
            foreach(var item in Rates)
                Rate += item.Rate; 
           
            if(Rates.Count()>0)
                finalRate = Rate / Rates.Count();  

            Result.Data = finalRate;
            Result.IsSucess = true;
            return Result;
        }


    }
}
