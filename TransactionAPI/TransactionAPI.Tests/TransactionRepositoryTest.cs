using Shared.Interfaces;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TransactionAPI.Data.Repository.v1;
using Xunit;

namespace TransactionAPI.Tests
{
    public class TransactionRepositoryTest
    {
        private readonly ITransactionRepository _repository;

        public TransactionRepositoryTest(ITransactionRepository repository)
        {
            _repository = repository;
        }

        [Fact]
        public async void CreateTransaction()
        {
            var testModel = new Transaction
            {
                Id = Guid.NewGuid(),
                Description = "Test"
            };

            var result = await _repository.AddAsync(testModel);
            Assert.NotNull(result);
            Assert.Equal(testModel.Id, result.Id);

            var list = _repository.GetAll();
            Assert.Single(list);
        }
    }
}