using System;
using System.Collections.Generic;
using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Services;
using Moq;
using NUnit.Framework;

namespace Tests;

[TestFixture]
public class TestEndWorkDayService
{
    private EndWorkDayService _service = null!;

    [SetUp]
    public void Setup()
    {
        var context = new Mock<IApplicationDbContext>();
        var serviceMock = new Mock<EndWorkDayService>(context.Object);
        serviceMock.Setup(x => x.GetWorkDays(It.IsAny<DateTime>()))
            .ReturnsAsync(new List<WorkDay>
                {
                    new()
                    {
                        StartDate = new DateTime(2022, 4, 19, 9, 0, 0),
                        Date = new DateTime(2022, 4, 20)
                    }
                });
        
        _service = serviceMock.Object;
    }

    [Test]
    public void Test1()
    {
        var date = new DateTime(2022, 4, 19, 23, 59, 59);
        var workDays = _service.GetWorkDays(date).Result;
        _service.EndWorkDays().Wait();

        Assert.AreEqual(date, workDays[0].EndDate);
    }
}