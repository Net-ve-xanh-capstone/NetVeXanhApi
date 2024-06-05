﻿using Domain.Models.Base;

namespace Domain.Models
#nullable disable warnings
{
    public partial class Account : BaseModel
    {
        
        public DateTime Birthday { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Avatar { get; set; }
        public bool Gender { get; set; } = true;


        //Relation
        public ICollection<Collection> Collection { get; set; }
        public ICollection<Contest> CreateContest { get; set; }
        public Guardian Guardian { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<Post> Post { get; set; }
        public ICollection<Schedule> Schedule {  get; set; }
        public ICollection<Painting> Painting { get; set; }
    }
}