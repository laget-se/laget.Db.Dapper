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

        //[Fact]
        //public void ShouldInsertItem()
        //{
        //    var season = new Season { Name = "1859", Visible = 2 };
        //    var output = _repository.Insert(season);
        //}

        //[Fact]
        //public void ShouldInsertItems()
        //{
        //    var domain = new Domain { ClubId = 67347, Name = "temp.co" };
        //    var output =  _repository.Insert(domain);
        //}

        //[Fact]
        //public void ShouldInsertMultipleItems()
        //{
        //    var domains = new List<Domain>
        //    {
        //        new Domain { ClubId = 67347, Name = "temp.co" },
        //        new Domain { ClubId = 67347, Name = "temp.se" },
        //        new Domain { ClubId = 67347, Name = "temp.uk" }
        //    };

        //    _repository.Insert(domain);
        //}

        //[Fact]
        //public void ShouldUpdateItem()
        //{
        //    var domain = _repository.Find("Name LIKE 'temp.co'").First();

        //    domain.SiteId = 1337;

        //    _repository.Update(domain);
        //}

        //[Fact]
        //public void ShouldUpdateItems()
        //{
        //    var domains = _repository.Find("Name LIKE '%temp%'");

        //    foreach (var domain in domains)
        //    {
        //        domain.SiteId = 134049;
        //    }

        //    _repository.Update(domains);
        //}

        //[Fact]
        //public void ShouldDeleteMultipleItems()
        //{
        //    var domains = new List<Domain>
        //    {
        //        new Domain { Id = 1333 },
        //        new Domain { Id = 1334 },
        //        new Domain { Id = 1335 }
        //    };

        //    _repository.Delete(domains);
        //}
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

    [DapperTable("tSeason")]
    public class Season : Entity
    {
        [DapperColumn("intSeasonId")]
        public override int Id { get; set; }
        [DapperColumn("strSeasonName")]
        public string Name { get; set; }
        [DapperColumn("intSeasonVisible")]
        public int Visible { get; set; }

        public override object ToObject() => new
        {
            Name,
            Visible
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
