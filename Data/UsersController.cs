using CoreApiInNet.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoreApiInNet.Contracts;
using CoreApiInNet.Repository;
using CoreApiInNet.Configurations;
using System.Reflection;
using AutoMapper;

namespace CoreApiInNet.Data
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {

        public InterfaceUserRepository UserRepository;

        public IMapper Mapper;

        public UsersController(InterfaceUserRepository userRepository, IMapper mapper)
        {
            UserRepository = userRepository;
            Mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DbModelUser>>> GetAll()
        {
            var users = await UserRepository.GetAllAsync();
            return Ok(Mapper.Map<List<DbModelUser>>(users));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DbModelUser>> GetById(int id)
        {
            if (UserRepository.GetAsync == null)
            {
                return NotFound();
            }
            var dbModelUser = await UserRepository.GetAsync(id);
            if (dbModelUser == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<DbModelUser>(dbModelUser));
        }

        [HttpPost]
        public async Task<IActionResult> Create(HelpingModelUser model)
        {
            using (var transaction = await UserRepository.StartTransaction())
            {
                /*DbModelUser dbModelUser = new DbModelUser();
                dbModelUser.Name = model.Name;
                dbModelUser.password = model.password;*/
                var user = Mapper.Map<DbModelUser>(model);
                await UserRepository.AddAsync(user);
                await transaction.CommitAsync();
                return CreatedAtAction(nameof(GetById), new { id = user.ID }, user);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(DbModelUser dbModelUser)
        {
            // Pobierz istniejącego użytkownika
            /*var existingUser = await UserRepository.GetAsync(dbModelUser.ID);
            if (existingUser == null)
            {
                return NotFound();
            }

            using var transaction = await UserRepository.StartTransaction();
            try
            {
                // Aktualizuj właściwości istniejącego użytkownika
                // Możesz użyć AutoMapper lub zaktualizować ręcznie
                existingUser.Name = dbModelUser.Name;
                existingUser.password = dbModelUser.password;
                // ... zaktualizuj pozostałe właściwości

                await UserRepository.UpdateAsync(existingUser);
                await transaction.CommitAsync();
                return Ok(existingUser);
            }
            catch
            {
                await transaction.RollbackAsync();
                // Logowanie błędu
                return StatusCode(500, "");
            }*/
            var dbModelUserr = await UserRepository.GetAsync(dbModelUser.ID) ;

            if (dbModelUserr == null)
            {
                return NotFound();
            }

            using (var transaction = await UserRepository.StartTransaction())
            {

                /*dbModelUserr.Name=dbModelUser.Name;
                dbModelUserr.password=dbModelUser.password;*/

                Mapper.Map(dbModelUser, dbModelUserr);

                await UserRepository.UpdateAsync(dbModelUserr);
                await transaction.CommitAsync();
                return Ok(dbModelUserr);

            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            using (var transaction = await UserRepository.StartTransaction())
            {
                var dbModelUser = await UserRepository.GetAsync(id);
                if (dbModelUser == null)
                {
                    await transaction.CommitAsync();
                    return NotFound();
                }
                UserRepository.DeleteAsync(id);
                await transaction.CommitAsync();
                return Ok();
            }


        }
    }
}
