﻿using Relativity.Services;
using Relativity.Services.ResourcePool;
using Relativity.Services.ResourceServer;
using Relativity.Services.ServiceProxy;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Helpers
{
	public class AgentServerHelper : IAgentServerHelper
	{
		private ServiceFactory ServiceFactory { get; }
		public AgentServerHelper(IConnectionHelper connectionHelper)
		{
			ServiceFactory = connectionHelper.GetServiceFactory();
		}

		/// <summary>
		/// Add Agent Server to Default Resource Pool
		/// </summary>
		/// <returns></returns>
		public async Task<bool> AddAgentServerToDefaultResourcePool()
		{
			bool wasAgentServerAddedToDefaultPool = false;

			Console.WriteLine($"{nameof(AddAgentServerToDefaultResourcePool)} - Adding Agent Server ({Constants.AgentServer.AgentServerName}) to the Default Resource Pool");

			// Setup for checking Resource Pools
			Relativity.Services.TextCondition conditionPool = new Relativity.Services.TextCondition()
			{
				Field = Constants.Processing.NameField,
				Operator = Relativity.Services.TextConditionEnum.StartsWith,
				Value = Constants.Processing.DefaultPool
			};

			Relativity.Services.Query queryPool = new Relativity.Services.Query()
			{
				Condition = conditionPool.ToQueryString(),
			};

			// Setup for checking if Agent Server exists
			Relativity.Services.TextCondition conditionAgent = new Relativity.Services.TextCondition()
			{
				Field = Constants.Processing.NameField,
				Operator = Relativity.Services.TextConditionEnum.EqualTo,
				Value = Constants.Processing.ResourceServerName
			};

			Relativity.Services.Query queryAgent = new Relativity.Services.Query()
			{
				Condition = conditionAgent.ToQueryString()
			};

			using (IResourceServerManager resourceServerManager = ServiceFactory.CreateProxy<IResourceServerManager>())
			using (IResourcePoolManager resourcePoolManager = ServiceFactory.CreateProxy<IResourcePoolManager>())
			{
				// Check for Default Resource Pool
				ResourcePoolQueryResultSet resultPools = await resourcePoolManager.QueryAsync(queryPool);

				Console.WriteLine($"{nameof(AddAgentServerToDefaultResourcePool)} - Checking if Default Resource Pool exists");
				if (resultPools.Success && resultPools.TotalCount > 0)
				{
					ResourcePoolRef defaultPoolRef = new ResourcePoolRef(resultPools.Results.Find(x => x.Artifact.Name.Equals(Constants.Processing.DefaultPool, StringComparison.OrdinalIgnoreCase)).Artifact.ArtifactID);

					List<ResourceServerRef> resultServers = await resourcePoolManager.RetrieveResourceServersAsync(defaultPoolRef);


					// Check to make sure the Agent Server was not already added
					if (!resultServers.Exists(x => x.ServerType.Name.Equals(Constants.AgentServer.AgentServerName, StringComparison.OrdinalIgnoreCase)))
					{
						// Make sure the Agent Server actually exists and then add it
						ResourceServerQueryResultSet queryResult = await resourceServerManager.QueryAsync(queryAgent);

						if (queryResult.Success && queryResult.TotalCount > 0)
						{
							ResourceServer agentServer = queryResult.Results.Find(x => x.Artifact.ServerType.Name.Equals(Constants.AgentServer.AgentServerName, StringComparison.OrdinalIgnoreCase)).Artifact;

							ResourceServerRef agentServerRef = new ResourceServerRef()
							{
								ArtifactID = agentServer.ArtifactID,
								Name = agentServer.Name,
								ServerType = new ResourceServerTypeRef()
								{
									ArtifactID = agentServer.ServerType.ArtifactID,
									Name = agentServer.ServerType.Name
								}
							};

							await resourcePoolManager.AddServerAsync(agentServerRef, defaultPoolRef);

							resultServers = await resourcePoolManager.RetrieveResourceServersAsync(defaultPoolRef);

							if (resultServers.Exists(x => x.ServerType.Name.Equals(Constants.AgentServer.AgentServerName, StringComparison.OrdinalIgnoreCase)))
							{
								wasAgentServerAddedToDefaultPool = true;
								Console.WriteLine($"{nameof(AddAgentServerToDefaultResourcePool)} - Successfully added Agent Server to Default Resource Pool");
							}
							else
							{
								wasAgentServerAddedToDefaultPool = false;
								Console.WriteLine($"{nameof(AddAgentServerToDefaultResourcePool)} - Failed to add Agent Server to Default Resource Pool.");
							}
						}
						else
						{
							Console.WriteLine($"{nameof(AddAgentServerToDefaultResourcePool)} - Failed to add Agent Server to Default Resource Pool as the Agent Server does not exist");
						}
					}
					else
					{
						wasAgentServerAddedToDefaultPool = true;
						Console.WriteLine($"{nameof(AddAgentServerToDefaultResourcePool)} - Failed to add Agent Server to Default Resource Pool as it already exists within the pool");
					}

				}
			}

			return wasAgentServerAddedToDefaultPool;
		}
	}
}