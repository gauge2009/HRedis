﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HRedis.UnitTest
{
    [TestClass]
    public class PoolRedisClientTests
    {
        private const string ip = "127.0.0.1";
        private const int port = 6381;

        private PoolRedisClient prc = new PoolRedisClient(new PoolConfiguration()
        {
            Host = ip,
            Port = port,
            MaxClients = 100
        });

        [TestMethod, TestCategory("poolRedisclient")]
        public void GetClient_Test()
        {
            using (var client = prc.GetClient())
            {
                client.Set("GetClient_Test", "GetClient_Test");

                var info2 = client.Get("GetClient_Test");

                Assert.AreEqual(info2.ToString(), "GetClient_Test");
            }
        }

        [TestMethod, TestCategory("poolRedisclient")]
        public void GetMaxClient_Test()
        {
            for (int i = 0; i < 100; i++)
            {
                using (var client = prc.GetClient())
                {
                    client.Set("GetMaxClient_Test" + i, "GetMaxClient_Test");

                    var info2 = client.Get("GetMaxClient_Test" + i);

                    Assert.AreEqual(info2.ToString(), "GetMaxClient_Test");
                }
            }
            prc.Dispose();
        }

        [TestMethod, TestCategory("poolRedisclient")]
        public void Parallel_PoolClient_Test()
        {
            Parallel.For(0, 100, (index,item) =>
            {
                using (var client = prc.GetClient())
                {
                    client.Set("Parallel_PoolClient_Test" + index, "Parallel_PoolClient_Test");

                    var info2 = client.Get("Parallel_PoolClient_Test" + index);

                    Assert.AreEqual(info2.ToString(), "Parallel_PoolClient_Test");
                }

            });
                
            prc.Dispose();
        }
    }
}