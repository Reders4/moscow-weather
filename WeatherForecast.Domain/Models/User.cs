﻿namespace WeatherForecast.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] SecurityKey { get; set; }
    }
}
