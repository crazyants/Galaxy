﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Galaxy.Events;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomerSample.API.Host.Controllers
{
    [Route("/")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {

        [HttpGet]
        [Route("healthcheck")]
        public IActionResult HealthCheck() => Ok(new { Status = "Healty" });

    }

    public class TestIntegerationEvent : IntegrationEvent
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}