﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BlazorSozluk.Infrastructure.Persistence.Context;

namespace BlazorSozluk.Infrastructure.Persistence.EntityConfiguration.Entry
{
    public class EntryEntityConfiguration:BaseEntityConfiguration<Api.Domain.Models.Entry>
    {
        public override void Configure(EntityTypeBuilder<Api.Domain.Models.Entry> builder)
        {
            base.Configure(builder);
            builder.ToTable("entry",BlazorSozlukContext.DEFAULT_SCHEMA);

            builder.HasOne(i => i.CreatedBy)
                .WithMany(i=>i.Entries)
                .HasForeignKey(i=>i.CreatedById);
        }
    }
}
