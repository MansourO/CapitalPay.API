using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shared.Interfaces;
using Shared.Models;
using Shared.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TransactionAPI.Controllers.v1;
using TransactionAPI.Data.Database;
using TransactionAPI.Data.Repository.v1;
using TransactionAPI.Service.v1.Services;
using Xunit;
using Xunit.Abstractions;

namespace TransactionAPI.Tests
{
    public class TransactionControllerTest
    {
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private TransactionController _controller;
        private readonly ITestOutputHelper _output;

        public TransactionControllerTest(IMapper mapper, ITransactionService transactionService, ITestOutputHelper testOutput)
        {
            _mapper = mapper;
            _transactionService = transactionService;
            _controller = new TransactionController(_mapper, _transactionService);
            _output = testOutput;
        }

        [Fact]
        public void GetTransactions_Test()
        {
            //arrange

            //act
            var result = _controller.GetTransactions();

            ////assert
            Assert.IsType<ActionResult<IEnumerable<Transaction>>>(result.Result);
        }

        [Fact]
        public async void CreateTransaction_Test()
        {
            //arrange
            var testModel = new PostTransaction
            {
                Description = "Test"
            };

            //assert
            var result = await _controller.CreateTransaction(testModel);

            //act
            Assert.IsType<ActionResult<Transaction>>(result);
        }
    }
}