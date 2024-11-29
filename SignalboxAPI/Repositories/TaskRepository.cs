using Shared.Models;
using Signalbox.Shared.Models;
using SignalboxAPI.DatabaseModels;
using SignalboxAPI.Interfaces;
using SignalboxAPI.Services;

namespace SignalboxAPI.Repositories
{
	public class TaskRepository : ITaskRepository
	{
		private readonly IDatabaseConnector _databaseConnector;
		private readonly IGroupRepository _groupRepository;
		public TaskRepository(IDatabaseConnector databaseConnector, IGroupRepository groupRepository)
		{
			this._databaseConnector = databaseConnector;
			_groupRepository = groupRepository;
		}

		public UserTask CreateTaskFromGroupId(int groupId, string taskName, int taskTypeCode)
		{
			GroupTasks groupTasks = GetTasksFromGroupId(groupId);

			if (groupTasks.Tasks.FindIndex(t => t.Name == taskName) != -1)
			{
				throw new Exception();
			}

			SqlRequest taskRequest = new SqlRequest("tblTask");

			SqlData nameData = new SqlData("tblTask", "Name", taskName);
			taskRequest.RequestValues.Add(nameData);

			SqlData groupIdData = new SqlData("tblTask", "GroupCode", groupId);
			taskRequest.RequestValues.Add(groupIdData);

			SqlData taskTypeData = new SqlData("tblTask", "TaskTypeCode", taskTypeCode);
			taskRequest.RequestValues.Add(taskTypeData);

			_databaseConnector.InsertData(taskRequest);

			groupTasks = GetTasksFromGroupId(groupId);

			return groupTasks.Tasks.Where(t => t.Name == taskName).First();
		}

		public UserTask GetTaskDetailsFromId(int taskId)
		{
			SqlRequest taskRequest = new SqlRequest("tblTask");

			SqlJoin typeJoin = new SqlJoin("tblTask", "TaskTypeCode", "tblTaskType", "TaskTypeId");
			taskRequest.Joins.Add(typeJoin);

			SqlData typeData = new SqlData("tblTask", "TaskId", taskId);
			taskRequest.RequestValues.Add(typeData);

			List<UserTask> task = _databaseConnector.SelectData<UserTask>(taskRequest);

			if (task.Count() == 0)
			{
				throw new Exception();
			}

			return task[0];
		}

		public GroupTasks GetTasksFromGroupId(int groupId)
		{
			SqlRequest tasksRequest = new SqlRequest("tblTask");

			SqlJoin typeJoin = new SqlJoin("tblTask", "TaskTypeCode", "tblTaskType", "TaskTypeId");
			tasksRequest.Joins.Add(typeJoin);

			SqlData taskData = new SqlData("tblTask", "GroupCode", groupId);
			tasksRequest.RequestValues.Add(taskData);

			List<UserTask> tasks = _databaseConnector.SelectData<UserTask>(tasksRequest);


			SqlRequest groupRequest = new SqlRequest("tblGroup");

			SqlData groupData = new SqlData("tblGroup", "GroupId", groupId);
			groupRequest.RequestValues.Add(groupData);

			List<Group> group = _databaseConnector.SelectData<Group>(groupRequest);

			if (group.Count() == 0)
			{
				throw new Exception();
			}

			GroupTasks returnGroupTasks = new GroupTasks();

			returnGroupTasks.Tasks = tasks;

			returnGroupTasks.GroupId = groupId;
			returnGroupTasks.Name = group[0].Name;

			return returnGroupTasks;
		}

		public List<User> GetTaskOwnerFromId(int taskId)
		{
			SqlRequest userRequest = new SqlRequest("tblUser");

			SqlJoin currTaskJoin = new SqlJoin("tblUser", "UserId", "tblCurrTaskInfo", "Owner");
			userRequest.Joins.Add(currTaskJoin);

			SqlData taskData = new SqlData("tblCurrTaskInfo", "TaskCode", taskId);
			userRequest.RequestValues.Add(taskData);

			List<User> users = _databaseConnector.SelectData<User>(userRequest);

			if (users.Count() == 0)
			{
				throw new Exception();
			}

			return users;
		}

		private List<CurrTaskInfo> GetCurrentTaskInfoFromTaskId(int taskId)
		{
			SqlRequest taskInfoRequest = new SqlRequest("tblCurrTaskInfo");

			SqlData taskData = new SqlData("tblCurrTaskInfo", "TaskCode", taskId);
			taskInfoRequest.RequestValues.Add(taskData);

			List<CurrTaskInfo> currentTaskInfo = _databaseConnector.SelectData<CurrTaskInfo>(taskInfoRequest);

			return currentTaskInfo;
		}

		private bool UserCanDoTask(int taskId, int userId)
		{
			UserTask task = GetTaskDetailsFromId(taskId);
			List<User> usersInGroup = _groupRepository.GetAllUsersInGroup(task.GroupCode);

			if (usersInGroup.FindIndex(u => u.UserId == userId) != -1)
			{
				return true;
			}

			return false;
		}

		private bool GroupCanDoTask(int taskId, int groupId)
		{
			UserTask task = GetTaskDetailsFromId(taskId);
			
			if (task.GroupCode == groupId)
			{
				return true;
			}

			return false;
		}

		public void SetTaskOwnerFromUserId(int taskId, int userId)
		{
			List<CurrTaskInfo> currentTaskInfo = GetCurrentTaskInfoFromTaskId(taskId);
			CurrTaskInfo userTaskInfo = default;

			//Checks if user is in group
			if (UserCanDoTask(taskId, userId) == false)
			{
				throw new Exception();
			}

			//Finds the user if they are already added into db
			int userTaskInfoIndex = currentTaskInfo.FindIndex(c => c.Owner == userId);
			if (userTaskInfoIndex != -1)
			{
				userTaskInfo = currentTaskInfo[userTaskInfoIndex];
				currentTaskInfo.RemoveAt(userTaskInfoIndex);
			}

			//Deletes other rows
			if (currentTaskInfo.Count() > 0)
			{
				foreach (int currTaskId in currentTaskInfo.Select(t => t.CurrTaskInfoId))
				{
					_databaseConnector.DeleteData($"DELETE FROM tblCurrTaskInfo WHERE CurrTaskInfoId = '{currTaskId}';");
				}
			}

			//Adds a new row to the db for the TaskInfo
			if (userTaskInfo == default)
			{
				SqlRequest insertRequest = new SqlRequest("tblCurrTaskInfo");

				SqlData taskCodeData = new SqlData("tblCurrTaskInfo", "TaskCode", taskId);
				insertRequest.RequestValues.Add(taskCodeData);

				SqlData isOwnerData = new SqlData("tblCurrTaskInfo", "IsOwner", true);
				insertRequest.RequestValues.Add(isOwnerData);

				SqlData ownerData = new SqlData("tblCurrTaskInfo", "Owner", userId);
				insertRequest.RequestValues.Add(ownerData);

				SqlData completedData = new SqlData("tblCurrTaskInfo", "Completed", false);
				insertRequest.RequestValues.Add(completedData);

				_databaseConnector.InsertData(insertRequest);
			}

			//Updates old row (That it didn't remove) to set it so that IsOwner is true
			else if (userTaskInfo.IsOwner != true)
			{
				_databaseConnector.UpdateData($"UPDATE tblCurrTaskInfo SET IsOwner = 1 WHERE CurrTaskInfoId = {userTaskInfo.CurrTaskInfoId};");
			}
		}

		public void SetTaskOwnerFromGroupId(int taskId, int groupId)
		{
			if (GroupCanDoTask(taskId, groupId) == false)
			{
				throw new Exception();
			}

			List<CurrTaskInfo> currentTaskInfo = GetCurrentTaskInfoFromTaskId(taskId);
			List<User> groupUsers = _groupRepository.GetAllUsersInGroup(groupId);

			foreach (User user in groupUsers)
			{
				int index = currentTaskInfo.FindIndex(c => c.Owner == user.UserId);
				if (index == -1)
				{
					SqlRequest addUserRequest = new SqlRequest("tblCurrTaskInfo");

					SqlData taskCodeData = new SqlData("tblCurrTaskInfo", "TaskCode", taskId);
					addUserRequest.RequestValues.Add(taskCodeData);

					SqlData isOwnerData = new SqlData("tblCurrTaskInfo", "IsOwner", false);
					addUserRequest.RequestValues.Add(isOwnerData);

					SqlData ownerData = new SqlData("tblCurrTaskInfo", "Owner", user.UserId);
					addUserRequest.RequestValues.Add(ownerData);

					SqlData completedData = new SqlData("tblCurrTaskInfo", "Completed", false);
					addUserRequest.RequestValues.Add(completedData);

					_databaseConnector.InsertData(addUserRequest);
				}

				else if (currentTaskInfo[index].IsOwner == true)
				{
					_databaseConnector.UpdateData($"UPDATE tblCurrTaskInfo SET IsOwner = 0 WHERE CurrTaskInfoId = {currentTaskInfo[index].CurrTaskInfoId};");
				}
			}

		}
	}
}
