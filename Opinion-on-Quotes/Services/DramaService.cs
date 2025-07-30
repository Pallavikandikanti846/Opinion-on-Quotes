using Microsoft.EntityFrameworkCore;
using Opinion_on_Quotes.Data;
using Opinion_on_Quotes.Interfaces;
using Opinion_on_Quotes.Models;
using System;

namespace Opinion_on_Quotes.Services
{
    public class DramaService: IDramaServices
    {
        private readonly ApplicationDbContext _context;
        // dependency injection of database context
        public DramaService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<DramaDto>> ListDramas()
        {
            // all dramas
            List<Drama> dramas = await _context.Dramas
                .ToListAsync();
            // empty list of data transfer object DramaDto
            List<DramaDto> DramaDtos = new List<DramaDto>();
            // foreach drama record in database
            foreach (Drama Drama in dramas)
            {
                // create new instance of DramaDto, add to list
                DramaDtos.Add(new DramaDto()
                {
                    drama_id = Drama.drama_id,
                    title = Drama.title,
                    release_year = Drama.release_year,
                    genre = Drama.genre,
                    synopsis=Drama.synopsis
                });
            }
            // return DramaDtos
            return DramaDtos;

        }


        public async Task<DramaDto?> FindDrama(int id)
        {
            
            var drama = await _context.Dramas
                .FirstOrDefaultAsync(c => c.drama_id == id);

            // no drama found
            if (drama == null)
            {
                return null;
            }
            // create an instance of DramaDto
            DramaDto DramaDtos = new DramaDto()
            {
                drama_id = drama.drama_id,
                title = drama.title,
                release_year = drama.release_year,
                genre = drama.genre,
                synopsis = drama.synopsis
            };
            return DramaDtos;

        }


        public async Task<ServiceResponse> UpdateDrama(DramaDto DramaDto)
        {
            ServiceResponse serviceResponse = new();

            var dramaToUpdate = await _context.Dramas.FindAsync(DramaDto.drama_id);

            if (dramaToUpdate == null){
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add($"Drama with ID {DramaDto.drama_id} not found.");
                return serviceResponse;
            }

            // fields to update
            dramaToUpdate.title = DramaDto.title;
            dramaToUpdate.release_year = DramaDto.release_year;
            dramaToUpdate.genre = DramaDto.genre;
            dramaToUpdate.synopsis = DramaDto.synopsis;
            try
            {
                await _context.SaveChangesAsync();
                serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;
            }
            catch (DbUpdateConcurrencyException)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("An error occurred updating the record");
                return serviceResponse;
            }

            //serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;
            return serviceResponse;
        }


        public async Task<ServiceResponse> AddDrama(DramaDto DramaDto)
        {
            ServiceResponse serviceResponse = new();


            // Create instance of Drama
            Drama drama = new Drama()
            {
                drama_id = DramaDto.drama_id,
                title = DramaDto.title,
                release_year = DramaDto.release_year,
                genre = DramaDto.genre,
                synopsis = DramaDto.synopsis
            };
            // SQL Equivalent: Insert into Drama (..) values (..)

            try
            {
                _context.Dramas.Add(drama);
                var rows=await _context.SaveChangesAsync();
                if (rows > 0)
                {
                    serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
                    serviceResponse.CreatedId = drama.drama_id;
                }
                else
                {
                    serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                    serviceResponse.Messages.Add("Drama could not be added.");
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("There was an error adding the Drama.");
                if (ex.InnerException != null)
                {
                    serviceResponse.Messages.Add(ex.InnerException.Message);
                }
            }


            //serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            //serviceResponse.CreatedId = drama.drama_id;
            return serviceResponse;
        }


        public async Task<ServiceResponse> DeleteDrama(int id)
        {
            ServiceResponse response = new();
            // Drama must exist in the first place
            var Drama = await _context.Dramas.FindAsync(id);
            if (Drama == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("Drama cannot be deleted because it does not exist.");
                return response;
            }

            try
            {
                _context.Dramas.Remove(Drama);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("Error encountered while deleting the Drama");
                return response;
            }

            response.Status = ServiceResponse.ServiceStatus.Deleted;

            return response;

        }

        }
    }
