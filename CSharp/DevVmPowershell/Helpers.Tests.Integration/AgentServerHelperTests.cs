﻿using NUnit.Framework;
using System.Threading.Tasks;

namespace Helpers.Tests.Integration
{
	public class AgentServerHelperTests
	{
		private IAgentServerHelper Sut { get; set; }

		[SetUp]
		public void Setup()
		{
			IConnectionHelper connectionHelper = new ConnectionHelper(
				relativityInstanceName: TestConstants.RELATIVITY_INSTANCE_NAME,
				relativityAdminUserName: TestConstants.RELATIVITY_ADMIN_USER_NAME,
				relativityAdminPassword: TestConstants.RELATIVITY_ADMIN_PASSWORD,
				sqlAdminUserName: TestConstants.SQL_USER_NAME,
				sqlAdminPassword: TestConstants.SQL_PASSWORD);

			Sut = new AgentServerHelper(connectionHelper);
		}

		[TearDown]
		public void TearDown()
		{
			Sut = null;
		}

		[Test, Order(10)]
		[TestCase(true)]
		public async Task AddAgentServerToDefaultResourcePoolAsyncTest(bool expectedResult)
		{
			//Arrange

			//Act
			bool result = await Sut.AddAgentServerToDefaultResourcePoolAsync();

			//Assert
			Assert.That(result, Is.EqualTo(expectedResult));
		}

		[Test, Order(20)]
		[TestCase(true)]
		public async Task RemoveAgentServerFromDefaultResourcePoolAsyncTest(bool expectedResult)
		{
			//Arrange

			//Act
			bool result = await Sut.RemoveAgentServerFromDefaultResourcePoolAsync();

			//Assert
			Assert.That(result, Is.EqualTo(expectedResult));
		}
	}
}
