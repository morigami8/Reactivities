using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Edit
    {
        public class Command : IRequest
            {
                public Guid Id { get; set; }
                public string Title { get; set; }
                public string Description { get; set; }
                public string Category { get; set; }
                //DateTime is the only property that can be null because DateTime needs at least a null value
                public DateTime? Date { get; set; }
                public string City { get; set; }
                public string Venue { get; set; }
            }
        
            public class Handler : IRequestHandler<Command>
            {
              private readonly DataContext _context;
              public Handler(DataContext context)
              {
                _context = context;
              }
        
              //Unit is empty Object from Mediatr
              public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
              {
        
                //handler logic
                var activity = await _context.Activities.FindAsync(request.Id);

                //need a way to stop the request before returning back to the controller
                if(activity == null)
                    throw new Exception("Could not find activity");

                    activity.Title = request.Title ??   activity.Title;
                    activity.Description = request.Description ??   activity.Description;
                    activity.Category = request.Category ??   activity.Category;
                    activity.Date = request.Date ??   activity.Date;
                    activity.City = request.City ??   activity.City;
                    activity.Venue = request.Venue ??   activity.Venue;
        
                //SaveChangesAsync returns an Int, corresponding to how many items were added/updated;
                var success = await _context.SaveChangesAsync() > 0;
        
                if(success) return Unit.Value;
        
                throw new Exception("Problem saving changes");
              }
            }
    }
}