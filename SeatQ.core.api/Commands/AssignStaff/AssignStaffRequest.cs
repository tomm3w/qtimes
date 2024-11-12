using common.api.Commands;
using System;
using System.ComponentModel.DataAnnotations;

namespace SeatQ.core.api.Commands.AssignStaff
{
    public class AssignStaffRequest : ICommandRequest
    {
        public AssignRestaurantTableModel Model { get; private set; }
        public AssignStaffRequest(AssignRestaurantTableModel model)
        {
            Model = model;
        }
    }

    public class AssignRestaurantTableModel
    {
        [Required]
        public int? RestaurantChainId { get; set; }
        [Required]
        public int? TableId { get; set; }
        [Required]
        public Guid UserId { get; set; }
    }
}