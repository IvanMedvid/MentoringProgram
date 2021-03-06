﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MentoringProgramDAL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using MentoringProgram.Models;
using MentoringProgramDAL.Entities;

namespace MentoringProgram.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private IAddressRepository _addressRepository;
        private IMapper _mapper;

        public AddressController(IAddressRepository addressRepository, IMapper mapper)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
        }

        [HttpGet()]
        public IActionResult GetAdresses()
        {
            var addressesFromRepo = _addressRepository.GetAddressList();

            var addresses = _mapper.Map<IEnumerable<AddressDto>>(addressesFromRepo);
            return Ok(addresses);
        }

        [HttpGet("{id}", Name ="GetAddress")]
        public IActionResult GetAddressById(int id)
        {
            var addressFromRepo = _addressRepository.GetAddressById(id);

            if (addressFromRepo == null)
            {
                return NotFound();
            }

            var address = _mapper.Map<AddressDto>(addressFromRepo);
            return Ok(address);
        }

        [HttpPost()]
        public IActionResult AddAddress([FromBody]AddressToManipulateDto address)
        {
            if (address == null)
            {
                return BadRequest();
            }

            var addressEntity = _mapper.Map<Address>(address);

            _addressRepository.AddAddress(addressEntity);

            if (!_addressRepository.Save())
            {
                throw new Exception("Creating an address failed on save.");
            }

            var addressToReturn = _mapper.Map<AddressDto>(addressEntity);

            return CreatedAtRoute("GetAddress",
                new { id = addressToReturn.Id },
                addressToReturn);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAddress(int id, AddressToManipulateDto address)
        {
            if (address == null)
            {
                return BadRequest();
            }

            var addressFromRepo = _addressRepository.GetAddressById(id);
            if (addressFromRepo == null)
            {
                return NotFound();
            }

            _mapper.Map(address, addressFromRepo);

            _addressRepository.UpdateAddress(addressFromRepo);

            if (!_addressRepository.Save())
            {
                throw new Exception($"Updating address {id} failed on save.");
            }

            return CreatedAtRoute("GetAddress",
                new { id = addressFromRepo.Id },
                address);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAddress(int id)
        {
            var addressFromRepo = _addressRepository.GetAddressById(id);
            if (addressFromRepo == null)
            {
                return NotFound();
            }

            _addressRepository.DeleteAddress(id);

            if (!_addressRepository.Save())
            {
                throw new Exception($"Deleting address {id} failed on save.");
            }

            return NoContent();
        }
    }
}