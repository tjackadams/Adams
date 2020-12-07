using System;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Adams.Domain;
using Adams.Services.Smoking.Domain.AggregatesModel.RecipeAggregate;
using Adams.Services.Smoking.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Adams.Services.Smoking.Infrastructure
{
    public sealed class SmokingContext : DbContext, IUnitOfWork
    {
        public const string DEFAULT_SCHEMA = "smoker";
        private IDbContextTransaction _currentTransaction;

        public SmokingContext(DbContextOptions<SmokingContext> options)
            : base(options)
        {
            Debug.WriteLine("SmokingContext::ctor ->" + GetHashCode());
        }

        public DbSet<Recipe> Recipes { get; set; }

        public bool HasActiveTransaction => _currentTransaction != null;

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            _ = await SaveChangesAsync(cancellationToken);

            return true;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RecipeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RecipeStepEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProteinEntityTypeConfiguration());
        }

        public IDbContextTransaction GetCurrentTransaction()
        {
            return _currentTransaction;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                return null;
            }

            _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            return _currentTransaction;
        }

        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }

            if (transaction != _currentTransaction)
            {
                throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");
            }

            try
            {
                await SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
    }
}