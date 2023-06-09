﻿using Microsoft.EntityFrameworkCore;
using RideShare.Application;
using RideShare.Application.Exceptions;
using RideShare.Application.UseCases.Commands.Create;
using RideShare.Application.UseCases.DTOs.Create;
using RideShare.DataAccess;
using RideShare.Domain.Entities;
using RideShare.Implementation.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideShare.Implementation.UseCases.Commands.Create
{
    public class EfSendRequestCommand : ISendRequestCommand
    {
        private readonly RideshareContext _context;
        private readonly IApplicationActor _actor;

        public EfSendRequestCommand(RideshareContext context, IApplicationActor actor)
        {
            _context = context;
            _actor = actor;
        }

        public int Id => 250;

        public string Name => "Send a request for a ride using Entity Framework";

        public void Execute(SendRequestDto request)
        {
            var ride = _context.Rides.Include(x => x.RideRequests).Include(x => x.Car).WhereActive().FirstOrDefault(x => x.Id == request.RideId);

            if (ride == null)
            {
                throw new EntityNotFoundException(request.RideId, nameof(Ride));
            }

            if(ride.DriverId == _actor.Id)
            {
                throw new InvalidOperationException("Cannot send a request to yourself.");
            }

            if (ride.StartDate < DateTime.Now)
            {
                throw new InvalidOperationException("Cannot send a request for a ride in the past.");
            }
            if (ride.RideRequests.Any(x => x.FromUserId == _actor.Id))
            {
                throw new InvalidOperationException("You have already sent a request.");
            }
            if (ride.RideRequests.Count > ride.Car.NumberOfSeats)
            {
                throw new InvalidOperationException("You cannot send a request for a ride with no available seats.");
            }

            var rideRequest = new RideRequest
            {
                RideId = ride.Id,
                FromUserId = _actor.Id,
                ToUserId = ride.DriverId,
                Status = RideStatus.Sent
            };

            _context.RideRequests.Add(rideRequest);
            _context.SaveChanges();
        }
    }
}
