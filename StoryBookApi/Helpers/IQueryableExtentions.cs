using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoryBookApi.DTOs;

namespace StoryBookApi.Helpers
{
    public static class IQueryableExtentions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, PaginationDto paginationDto)
        {
            return queryable
                .Skip((paginationDto.Page - 1) * paginationDto.RecordsPerPage)
                .Take(paginationDto.RecordsPerPage);
        }
    }
}
