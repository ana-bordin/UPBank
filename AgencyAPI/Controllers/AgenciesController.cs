﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgencyAPI.Data;
using Models;
using AgencyAPI.Services;
using Models.DTO;
using NuGet.Protocol;

namespace AgencyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgenciesController : ControllerBase
    {
        private readonly AgencyAPIContext _context;
        private readonly AddressService _addressService;
        private readonly EmployeeService _employeeService;
        private readonly AccountService _accountService;

        public AgenciesController(AgencyAPIContext context, AddressService address, EmployeeService employee, AccountService account)
        {
            _context = context;
            _addressService = address;
            _employeeService = employee;
            _accountService = account;
        }

        // POST: api/Agencies
        [HttpPost]
        public async Task<ActionResult<Agency>> PostAgency(AgencyDTO agencyDTO)
        {
            Agency agency = new();
            List<Employee> employees = new();

            if (_context.Agency == null)
            {
                return Problem("Entity set 'AgencyAPIContext.Agency'  is null.");
            }

            foreach (var employee in agencyDTO.Employees)
            {
                if (employee == null)
                    return BadRequest("Employee not found.");
                else
                {
                    employees.Add(await _employeeService.GetEmployee(employee));
                }
            }

            if (employees == null)
                return BadRequest("Employee not found.");

            else if (!(employees.Find(e => e.Manager).Manager))
                return BadRequest("The first employee must be a manager.");

            else
                agency.Employees = employees;

            Address address = await _addressService.GetAddress(agencyDTO.Address.ZipCode);
            address.Number = agencyDTO.Address.Number;
            address.Complement = agencyDTO.Address.Complement;

            if (address == null)
                return BadRequest("Address not found.");

            else
                agency.Address = address;

            _context.Agency.Add(agency);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                if (AgencyExists(agency.Number))
                {
                    return Conflict();
                }
                else
                {
                    return BadRequest(e.Message);
                }
            }
                    return CreatedAtAction("GetAgency", new { id = agency.Number }, agency);
        }

        // PUT: api/Agencies/5
        [HttpPut("{number}")]
        public async Task<IActionResult> PutAgency(string number, AgencyPatchDTO agencyPatchDTO)
        {
            var agency = await _context.Agency.FindAsync(number);

            if (agencyPatchDTO.Restriction != null)
                agency.Restriction = agencyPatchDTO.Restriction;

            if (!agency.Restriction)
            {
                if (number != agency.Number)
                    return BadRequest("The agency number is invalid.");

                if (agencyPatchDTO.Address != null && (agency.Address.ZipCode != agencyPatchDTO.Address.ZipCode || agency.Address.Number != agencyPatchDTO.Address.Number || agency.Address.Complement != agencyPatchDTO.Address.Complement))
                {
                    agency.Address = await _addressService.GetAddress(agencyPatchDTO.Address.ZipCode);
                    agency.Address.Number = agencyPatchDTO.Address.Number;
                    agency.Address.Complement = agencyPatchDTO.Address.Complement;
                }

                if (agencyPatchDTO.Employees != null)
                {
                    foreach (var employee in agencyPatchDTO.Employees)
                        agency.Employees.Add(await _employeeService.GetEmployee(employee));
                }
                _context.Update(agency);
                //_context.Entry(duvida).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    if (!AgencyExists(number))
                    {
                        return NotFound("Agency not found.");
                    }
                    else
                    {
                        return BadRequest(e.Message);
                    }
                }
            }

            else
            {
                _context.Update(agency); 
                await _context.SaveChangesAsync();
                return BadRequest("The agency is restricted.");
            }
                

            return NoContent(); 
        }

        // GET: api/Agencies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Agency>>> GetAgency()
        {
            if (_context.Agency == null)
            {
                return NotFound();
            }
            return await _context.Agency.Include(a => a.Address).Include(e => e.Employees).ToListAsync();
        }

        // GET: api/Agencies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Agency>> GetAgency(string number)
        {
            if (_context.Agency == null)
            {
                return NotFound();
            }
            var agency = await _context.Agency.Include(a => a.Address).Include(e => e.Employees).Where(c => c.Number == number).SingleOrDefaultAsync();

            if (agency == null)
            {
                return NotFound();
            }

            return agency;
        }

        // DELETE: api/Agencies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAgency(string number)
        {
            if (_context.Agency == null)
            {
                return NotFound();
            }
            var agency = await _context.Agency.FindAsync(number);
            if (agency == null)
            {
                return NotFound();
            }

            _context.Agency.Remove(agency);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AgencyExists(string id)
        {
            return (_context.Agency?.Any(e => e.Number == id)).GetValueOrDefault();
        }

        //Get: api/Agencies/RestrictedAccounts
        [HttpGet("RestrictAccounts")]
        public async Task<ActionResult<IEnumerable<Account>>> GetRestrictedAccounts()
        {
            return await _accountService.GetRestrictedAccounts();
        }

        //Get: api/Agencies/AccountsPerProfile
        [HttpGet("AccountsPerProfile/{profile}")]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccountsPerProfile(string profile)
        {
            return await _accountService.GetAccountsPerProfile(profile);
        }
    }
}
