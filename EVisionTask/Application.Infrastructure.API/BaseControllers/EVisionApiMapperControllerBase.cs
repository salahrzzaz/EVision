using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Application.Infrastructure.Data.Base;
using Application.Infrastructure.Data.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Application.Infrastructure.API.BaseControllers
{
    [Route("api/[controller]")]
    public abstract class EVisionApiMapperControllerBase<T, TVm> : EVisionControllerBase
       where T : class, IEntity
    {
        protected readonly IMapper Mapper;
        protected readonly IRepository<T> Repository;

        protected EVisionApiMapperControllerBase(IRepository<T> repository, IMapper mapper,
            ILogger logger) : base(logger)
        {
            Repository = repository;
            Mapper = mapper;
        }

     

        #region API Methods

        /// <summary>
        ///     Returns list of items by page
        /// </summary>
        /// <returns>Returns a page of the items</returns>
        [HttpGet]
        [Route("page")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Produces("application/json")]
        public virtual async Task<IActionResult> Page(string search = "", string sortBy = "", SortingType sort = SortingType.Ascending, int pageNumber = 1, int pageSize = 10)
        {
            var (count, page) = await Repository.GetPage(pageNumber, pageSize, search, sortBy, sort);
            var result = Mapper.Map<IEnumerable<TVm>>(page);
            return Success("Entity Found", new
            {
                numberOfPages = count,
                list = result
            });
        }

        /// <summary>
        ///     Returns list of items
        /// </summary>
        /// <returns>Returns a list of all items</returns>
        [HttpGet]
        [Route("")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Produces("application/json")]
        public virtual async Task<IActionResult> List()
        {
            var list = await Repository.List();
            var result = Mapper.Map<IEnumerable<TVm>>(list);
            return Success("Entity Found", result);
        }

        /// <summary>
        /// Creates a new item
        /// </summary>
        /// <param name="model">The new item to create</param>
        /// <returns>Returns the newly created item</returns>
        [HttpPost]
        [Route("")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Produces("application/json")]
        public virtual async Task<IActionResult> Post(TVm model)
        {
            if (!ModelState.IsValid)
                return Failure(HttpStatusCode.BadRequest, "ValidationError",
                    ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                        .Select(e => e.ErrorMessage).ToList());
            var mappedModel = Mapper.Map<T>(model);
            var result = await Repository.Insert(mappedModel);
            var mappedResult = Mapper.Map<TVm>(result);
            return CreatedWithSuccess("Entity Found", mappedResult);
        }

        /// <summary>
        /// Updates an item
        /// </summary>
        /// <param name="id">The item ID {int}</param>
        /// <param name="model">The updated item body {include the id inside the body}</param>
        /// <returns>Returns the item after the update</returns>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Produces("application/json")]
        public virtual async Task<IActionResult> Update(int id, TVm model)
        {
            if (!ModelState.IsValid)
                return Failure(HttpStatusCode.BadRequest, "ValidationError",
                    ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                        .Select(e => e.ErrorMessage).ToList());
            var mappedModel = Mapper.Map<T>(model);
            mappedModel.Id = id;
            var result = await Repository.Update(mappedModel);
            var mappedResult = Mapper.Map<TVm>(result);
            return Success("Updated", mappedResult);
        }

        /// <summary>
        /// Deletes an item by ID
        /// </summary>
        /// <param name="id">Item ID {int}</param>
        /// <returns>Returns success if the delete operation completed successfully</returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Produces("application/json")]
        public virtual async Task<IActionResult> Delete(int id)
        {
            await Repository.Delete(id);
            return Success("Deleted", null);
        }

        /// <summary>
        ///     Returns an item by its ID
        /// </summary>
        /// <param name="id">The item's ID {int}</param>
        /// <returns>Returns the items by id</returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Produces("application/json")]
        public virtual async Task<IActionResult> Get(int id)
        {
            var model = await Repository.Get(id);
            var result = Mapper.Map<TVm>(model);
            return Success(result == null ? "not found" : "Entity Found", result);
        }

        #endregion
    }
}
