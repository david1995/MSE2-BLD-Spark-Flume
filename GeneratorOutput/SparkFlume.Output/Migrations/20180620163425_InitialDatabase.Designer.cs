﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using SparkFlume.Output.Entities;
using System;

namespace SparkFlume.Output.Migrations
{
    [DbContext(typeof(ProductDbContext))]
    [Migration("20180620163425_InitialDatabase")]
    partial class InitialDatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026");

            modelBuilder.Entity("SparkFlume.Output.Entities.Product", b =>
                {
                    b.Property<int>("Id");

                    b.Property<DateTime>("Minute");

                    b.Property<long>("Purchases");

                    b.Property<decimal>("Revenue");

                    b.Property<long>("Views");

                    b.HasKey("Id", "Minute");

                    b.ToTable("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
