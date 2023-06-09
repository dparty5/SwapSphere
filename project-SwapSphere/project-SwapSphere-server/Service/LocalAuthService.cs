﻿using project_SwapSphere_dblayer;
using project_SwapSphere_models;
using project_SwapSphere_models.Models;
using System.Collections.Generic;

namespace project_SwapSphere_server.Service
{
    public class LocalAuthService
    {

        public class Session
        {
            public Client User { get; set; }
            public DateTime LastOp { get; set; }
            public Guid Token { get; set; }
            public bool IsActive => DateTime.Now - LastOp < TimeSpan.FromHours(1);
        }

        public void CleanSession() =>
                Task.Run(() =>
                    Sessions.RemoveAll(x => !x.IsActive));

        //Singleton
        private static LocalAuthService? _instance;
        public static LocalAuthService GetInstance() =>
            _instance ?? (_instance = new LocalAuthService());
        private LocalAuthService() { }

        private List<Session> Sessions { get; set; } = new List<Session>();
        private EntityGateway db = new();
        public Guid Auth(string username, string password)
        {
            var pashash = Extentions.ComputeSha256Hash(password);

            var potentialUser = db.GetClients(x => x.Login == username && x.Password == pashash).FirstOrDefault() ??
                throw new Exception("User is not found");

            var token = Guid.NewGuid();
            Sessions.Add(new()
            {
                User = potentialUser,
                LastOp = DateTime.Now,
                Token = token
            });
            return token;
        }
        public Role GetRole(Guid token)
        {
            CleanSession();
            return Sessions.FirstOrDefault(x => x.Token == token)?.User.Role ?? throw new Exception("Session is not found");
        }

        public Client GetClient(Guid token)
        {
            CleanSession();
            return Sessions.FirstOrDefault(x => x.Token == token)?.User ?? throw new Exception("User is not found");
        }


    }
}
