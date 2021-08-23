using System;
using System.Collections.Generic;
using System.Linq;
using laget.Db.Dapper.Extensions;
using Xunit;

namespace laget.Db.Dapper.Tests
{
    public class TemporaryTests
    {
        private readonly ITemporaryRepository _repository;

        public TemporaryTests()
        {
            var provider = new DapperDefaultProvider("");
            _repository = new TemporaryRepository(provider);
        }

        [Fact(Skip = "These tests run actual database calls, we need to provide a way to test these methods without an actual database call!")]
        public void ShouldInsertMultipleItems()
        {
            var domains = new List<Domain>
            {
                new Domain { ClubId = 67347, Name = "temp.co" },
                new Domain { ClubId = 67347, Name = "temp.se" },
                new Domain { ClubId = 67347, Name = "temp.uk" }
            };

            _repository.Insert(domains);
        }

        [Fact(Skip = "These tests run actual database calls, we need to provide a way to test these methods without an actual database call!")]
        public void ShouldUpdateItem()
        {
            var domain = _repository.Find("Name LIKE 'temp.co'").First();

            domain.SiteId = 1337;

            _repository.Update(domain);
        }

        [Fact(Skip = "These tests run actual database calls, we need to provide a way to test these methods without an actual database call!")]
        public void ShouldUpdateItems()
        {
            var domains = _repository.Find("Name LIKE '%temp%'");

            foreach (var domain in domains)
            {
                domain.SiteId = 134049;
            }

            _repository.Update(domains);
        }

        [Fact(Skip = "These tests run actual database calls, we need to provide a way to test these methods without an actual database call!")]
        public void ShouldDeleteMultipleItems()
        {
            var domains = new List<Domain>
            {
                new Domain { Id = 1333 },
                new Domain { Id = 1334 },
                new Domain { Id = 1335 }
            };

            _repository.Delete(domains);
        }
    }

    [DapperTable("tAcmeDomain")]
    public class Domain : Entity
    {
        public int ClubId { get; set; }
        public int SiteId { get; set; }
        public string AccountId { get; set; }
        public string CertificateId { get; set; }
        public string Name { get; set; }
        public int Step { get; set; }
        public bool Failed { get; set; } = false;
        public bool Valid { get; set; } = false;
        public int Retries { get; set; }
        public DateTime? RenewedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public DateTime? RevokedAt { get; set; }

        public override object ToObject() => new
        {
            ClubId,
            SiteId,
            AccountId,
            CertificateId,
            Name,
            Valid,
            RenewedAt,
            ExpiresAt
        };
    }

    public interface ITemporaryRepository : IRepository<Domain>
    {
    }

    public class TemporaryRepository : Repository<Domain>, ITemporaryRepository
    {
        public TemporaryRepository(IDapperDefaultProvider provider)
            : base(provider)
        {
        }
    }
}
