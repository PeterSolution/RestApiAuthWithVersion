using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreApiInNet.Model;
using AutoMapper;
using CoreApiInNet.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Routing;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.AspNetCore.OData.Query;

namespace CoreApiInNet.Data
{
    [ApiController]
    [ApiVersion("2.0",Deprecated =true)]
    [Route("api/v{version:apiVersion}/datas")]
    public class DatasV2Controller : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly InterfaceDataRepository dataRepository;

        public ILogger Logger { get; set; }

        public DatasV2Controller(IMapper mapper, InterfaceDataRepository dataRepository,ILogger<DatasController> logger)
        {

            this.mapper = mapper;
            this.dataRepository = dataRepository;
            Logger = logger;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAll()
        {

            using (var transaction = await dataRepository.StartTransaction())
            {
                return dataRepository.GetAllAsync != null ?
                Ok(await dataRepository.GetAllAsync()) :
                Problem("Database is empty.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {

            using (var transaction = await dataRepository.StartTransaction())
            {
                if (dataRepository.GetAsync(id) == null)
                {
                    return NotFound();
                }

                var dbModelData = await dataRepository.GetAsync(id);
                if (dbModelData == null)
                {
                    return NotFound();
                }

                return Ok(dbModelData);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(HelpingModelData model)
        {
            Logger.LogInformation($"Insert: {model} date: {DateTime.Now}");
            if (!ModelState.IsValid)
            {
                Logger.LogWarning($"Error model: {model}");
                return BadRequest(ModelState);
            }
            var dbModelData = mapper.Map<DbModelData>(model);
            using (var transaction = await dataRepository.StartTransaction())
            {
                /*DbModelData dbModelDataold = new DbModelData();
                dbModelDataold.data = model.data;
                dbModelDataold.IdUser = model.IdUser;*/



                await dataRepository.AddAsync(dbModelData);
                await transaction.CommitAsync();
                return CreatedAtAction(nameof(GetById), new { id = dbModelData.IdData }, dbModelData);
            }
        }
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update(FullDataModel dbModelData)
        {

            using (var transaction = await dataRepository.StartTransaction())
            {
                try
                {

                    DbModelData dbmodel = await dataRepository.GetAsync(dbModelData.id);
                    if(dbmodel == null)
                    {
                        Logger.LogWarning($"Wrong id in put method");
                        return NotFound();
                    }
                    mapper.Map(dbModelData, dbmodel);
                    //_context.Update(dbModelData);
                    try
                    {
                        await dataRepository.UpdateAsync(dbmodel);
                    }
                    catch (Exception ex)
                    {
                        return NotFound(ex);
                    }
                    await transaction.CommitAsync();
                    return Ok(dbmodel);
                }
                catch (Exception ex)
                {
                    Logger.LogWarning($"Error: {ex.Message} ");
                    return NotFound();
                }
            }



        }

        [HttpDelete("{id}")]
        [Authorize(Roles ="Administrator")]
        public async Task<IActionResult> Delete(int id)
        {

            var dbModelData = await dataRepository.GetAsync(id);
            if (dbModelData == null)
            {
                Logger.LogWarning($"Delete error: model with this id do not exist");
                return NotFound();
            }
            using (var transaction = await dataRepository.StartTransaction())
            {
                await dataRepository.DeleteAsync(id);
                await transaction.CommitAsync();
                return Ok();
            }
        }

        /*private bool DbModelDataExists(int id)
        {
            return (_context.DataModel?.Any(e => e.IdData == id)).GetValueOrDefault();
        }*/
    }
}
