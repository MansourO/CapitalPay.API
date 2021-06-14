using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace TransactionAPI.Data.Database
{
    public class CapitalTransactionContext : DbContext
    {
        private static readonly MethodInfo _boolPropertyMethodInfo = typeof(EF).GetMethod(nameof(EF.Property), BindingFlags.Static | BindingFlags.Public).MakeGenericMethod(typeof(bool));

        /// <summary>
        /// entity is not deleted lamda expresssion
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private LambdaExpression NotDeleted(Microsoft.EntityFrameworkCore.Metadata.IMutableEntityType entity)
        {
            var parameter = Expression.Parameter(entity.ClrType);
            var propertyMethodInfo = _boolPropertyMethodInfo; //typeof(EF).GetMethod("Property").MakeGenericMethod(typeof(bool));
            var isDeletedProperty = Expression.Call(propertyMethodInfo, parameter, Expression.Constant("IsDeleted"));

            BinaryExpression compareExpression = Expression.MakeBinary(ExpressionType.Equal, isDeletedProperty, Expression.Constant(false));

            return Expression.Lambda(compareExpression, parameter);
        }

        public CapitalTransactionContext(DbContextOptions<CapitalTransactionContext> options)
            : base(options)
        { }

        //tables
        public virtual DbSet<Shared.Models.Transaction> Transactions { get; set; }

        public virtual DbSet<Shared.Models.TransactionCategory> TransactionCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // deleted query filter applies to all base entity types
            // this is a soft delete when record is marked deleted it
            // is still on the record
            foreach (var entity in modelBuilder.Model.GetEntityTypes().Where(x => typeof(Shared.Models.BaseModel).IsAssignableFrom(x.ClrType)))
            {
                modelBuilder.Entity(entity.ClrType).HasQueryFilter(NotDeleted(entity));
            }

            // turn off cascade delete on all foreign key relationships
            var foreignKeys = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(x => !x.IsOwnership && x.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in foreignKeys)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            //populate data
            modelBuilder.Entity<Shared.Models.TransactionCategory>().HasData(
                new Shared.Models.TransactionCategory
                {
                    Id = Guid.NewGuid(),
                    Name = "Utilities",
                    Description = "A category to group all utility transactions"
                },
                new Shared.Models.TransactionCategory
                {
                    Id = Guid.NewGuid(),
                    Name = "Food",
                    Description = "A category to group all food transactions"
                }
                ); ;
        }
    }
}