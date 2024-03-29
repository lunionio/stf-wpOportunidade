﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WpOportunidades.Domains;
using WpOportunidades.Helper;
using WpOportunidades.Infrastructure;
using WpOportunidades.Services;

namespace WpOportunidades
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<SegurancaService>(); 
            services.AddTransient<OportunidadeRepository>();
            services.AddTransient<EnderecoRepository>();
            services.AddTransient<OportunidadeDomain>();
            services.AddTransient<EnderecoDomain>();
            services.AddTransient<OportunidadeStatusDomain>(); 
            services.AddTransient<UserXOportunidadeRepository>(); 
            services.AddTransient<OportunidadeStatusRepository>();
            services.AddTransient<EmailHandler>();
            services.AddTransient<ConfiguracaoService>();
            services.AddTransient<EmailService>();

            var corsBuilder = new CorsPolicyBuilder();
            corsBuilder.AllowAnyHeader();
            corsBuilder.AllowAnyMethod();
            corsBuilder.AllowAnyOrigin();
            corsBuilder.AllowCredentials();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", corsBuilder.Build());
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowSpecificOrigin");

            app.UseMvc();
        }
    }
}
