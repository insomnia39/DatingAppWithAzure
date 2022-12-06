using System;
using System.Text.Json.Serialization;

namespace DatingApp.DAL.Model
{
    public class ModelBase
    {
        public string Id { set; get; } = Guid.NewGuid().ToString().ToLower();
        public string Partition { set; get; } = nameof(Partition);
    }
}
