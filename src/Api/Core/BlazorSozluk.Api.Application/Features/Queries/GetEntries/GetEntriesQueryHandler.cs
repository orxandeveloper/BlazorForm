using AutoMapper;
using AutoMapper.QueryableExtensions;
using BlazorSozluk.Api.Application.Interfaces.Repositories;
using BlazorSozluk.Common.Models.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlazorSozluk.Api.Application.Features.Queries.GetEntries
{
    public class GetEntriesQueryHandler : IRequestHandler<GetEntriesQuery, List<GetEntriesViewModel>>
    {
        readonly IEntryRepository entryRepository;
        readonly IMapper mapper;

        public GetEntriesQueryHandler(IEntryRepository entryRepository, IMapper mapper )
        {
            this.entryRepository = entryRepository;
            this.mapper = mapper;
        }

        public async Task<List<GetEntriesViewModel>> Handle(GetEntriesQuery request, CancellationToken cancellationToken)
        {
            var query = entryRepository.AsQueryable();
            if (request.TodaysEntries)
            {
                query = query.Where(x => x.CreateDate >= DateTime.Now.Date)
                             .Where(x => x.CreateDate <= DateTime.Now.AddDays(1).Date);
            }
            query.Include(x => x.EntryComments)
                .Take(request.Count)
                .OrderBy(x => Guid.NewGuid());
            return await query.ProjectTo< GetEntriesViewModel>(mapper.ConfigurationProvider).ToListAsync(cancellationToken);
        }
    }
}
