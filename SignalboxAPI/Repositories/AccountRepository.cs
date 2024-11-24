using Signalbox.Shared.Models;
using SignalboxAPI.Interfaces;
using SignalboxAPI.Services;
using Shared.Helpers;
using SignalboxAPI.DatabaseModels;

namespace SignalboxAPI.Repositories
{
	public class AccountRepository : IAccountRepository
	{
		private readonly IDatabaseConnector _databaseConnector;
		public AccountRepository(IDatabaseConnector databaseConnector)
		{
			this._databaseConnector = databaseConnector;
		}

		public User CreateAccount(string userName, string password, string name)
		{
			string hashedPassword = Hashing.Hash(userName.Substring(0, 3) + password);

			SqlData userNameData = new SqlData("tblUser", "UserName", userName);
			
			SqlRequest selectUserRequest = new SqlRequest("tblUser");
			selectUserRequest.RequestValues.Add(userNameData);

			if (_databaseConnector.SelectData<User>(selectUserRequest).Count() > 0)
			{
				throw new Exception();
			}

			//Creates SqlRequest and asks _databaseConnector to send it to the db
			SqlRequest passwordRequest = new SqlRequest("tblPassword");

			SqlData passwordData = new SqlData("tblPassword", "Password", hashedPassword);
			passwordRequest.RequestValues.Add(passwordData);

			_databaseConnector.InsertData(passwordRequest);

			//Selects the data from the database using the hashed password string
			SqlRequest selectPasswordRequest = new SqlRequest("tblPassword");
			selectPasswordRequest.RequestValues.Add(passwordData);
			List<PasswordData> passwords = _databaseConnector.SelectData<PasswordData>(selectPasswordRequest);
			if (password.Count() == 0)
			{
				throw new Exception();
			}

			//Creates request for inserting user into database
			SqlRequest userRequest = new SqlRequest("tblUser");

			userRequest.RequestValues.Add(userNameData);

			SqlData nameData = new SqlData("tblUser", "Name", name);
			userRequest.RequestValues.Add(nameData);

			SqlData passwordCodeData = new SqlData("tblUser", "PasswordCode", passwords[0].PasswordId);
			userRequest.RequestValues.Add(passwordCodeData);

			_databaseConnector.InsertData(userRequest);

			//Selects user using UserName
			User user = _databaseConnector.SelectData<User>(selectUserRequest)[0];
			return user;
		}


		public Password PasswordCorrect(string password, string userName)
		{
			Password response = new Password();

			string hashedPassword = Hashing.Hash(userName.Substring(0, 3) + password);

			SqlRequest userRequest = new SqlRequest("tblUser");
			SqlJoin passwordJoin = new SqlJoin("tblUser", "PasswordCode", "tblPassword", "PasswordId");
			userRequest.Joins.Add(passwordJoin);

			SqlData userNameData = new SqlData("tblUser", "UserName", userName);
			userRequest.RequestValues.Add(userNameData);

			SqlData passwordData = new SqlData("tblPassword", "Password", hashedPassword);
			userRequest.RequestValues.Add(passwordData);

			List <User> users = _databaseConnector.SelectData<User>(userRequest);
				
			if (users.Count() == 0)
			{
				response.UserCode = -1;
				response.Valid = false;
				response.Reason = "Incorrect Password.";
			}
			else
			{
				response.UserCode = users[0].UserId;
				response.Valid = true;
				response.Reason = "";
			}

			return response;
		}
	}
}
