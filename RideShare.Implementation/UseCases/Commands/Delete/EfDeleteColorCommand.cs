﻿using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RideShare.Application.Exceptions;
using RideShare.Application.UseCases.Commands.Delete;
using RideShare.DataAccess;
using RideShare.Domain.Entities;
using RideShare.Implementation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideShare.Implementation.UseCases.Commands.Delete
{
    public class EfDeleteColorCommand : IDeleteColorCommand
    {
        private readonly RideshareContext _context;
        private readonly DeleteColorValidator _validator;
        public EfDeleteColorCommand(RideshareContext context, DeleteColorValidator validator)
        {
            _context = context;
            _validator = validator;
        }

        public int Id => 53;

        public string Name => "Delete color using Entity Framework";

        public void Execute(int request)
        {
            _validator.ValidateAndThrow(request);
            /*var color = _context.Colors.Include(x => x.Cars).FirstOrDefault(x => x.Id == request);

            if (color == null)
            {
                throw new EntityNotFoundException(request, nameof(Color));
            }

            var colorIsBeingUsed = color.Cars.Any();

            if (colorIsBeingUsed)
            {
                throw new DeleteOperationException($"Cannot delete color. Car with {color.Name} exists");
            }*/
            var color = _context.Colors.Find(request);

            color.IsDeleted = true;
            color.DeletedAt = DateTime.UtcNow;
            color.IsActive = false;

            _context.SaveChanges();
        }
    }
}