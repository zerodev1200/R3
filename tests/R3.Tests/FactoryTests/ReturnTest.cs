﻿using Microsoft.Extensions.Time.Testing;

namespace R3.Tests.FactoryTests;

public class ReturnTest
{
    [Fact]
    public void Return()
    {
        {
            using var list = EventFactory.Return(10).LiveRecord();
            list.AssertEqual([10]);
            list.AssertIsCompleted();
        }
        {
            var fakeTime = new FakeTimeProvider();

            using var list = EventFactory.Return(10, TimeSpan.Zero, fakeTime).LiveRecord();
            list.AssertEqual([10]);
            list.AssertIsCompleted();
        }
        {
            var fakeTime = new FakeTimeProvider();

            using var list = EventFactory.Return(10, TimeSpan.FromSeconds(5), fakeTime).LiveRecord();
            list.AssertEqual([]);

            fakeTime.Advance(TimeSpan.FromSeconds(4));
            list.AssertEqual([]);
            list.AssertIsNotCompleted();

            fakeTime.Advance(TimeSpan.FromSeconds(1));
            list.AssertEqual([10]);
            list.AssertIsCompleted();
        }
    }

    [Fact]
    public void ReturnThreadPoolScheduleOptimized()
    {
        using var list = EventFactory.Return(10).LiveRecord();

        Thread.Sleep(1);

        list.AssertEqual([10]);
        list.AssertIsCompleted();
    }

    // return on completed
    [Fact]
    public void ReturnOnCompleted()
    {
        {
            using var list = EventFactory.Return(0, "foo").LiveRecord();
            list.AssertEqual([0]);
            list.AssertIsCompleted();
            list.AssertCompletedValue("foo");
        }
        {
            var fakeTime = new FakeTimeProvider();

            using var list = EventFactory.Return(10, "foo", TimeSpan.FromSeconds(5), fakeTime).LiveRecord();
            list.AssertEqual([]);

            fakeTime.Advance(TimeSpan.FromSeconds(4));
            list.AssertEqual([]);
            list.AssertIsNotCompleted();

            fakeTime.Advance(TimeSpan.FromSeconds(1));
            list.AssertEqual([10]);
            list.AssertIsCompleted();
            list.AssertCompletedValue("foo");
        }
    }
}