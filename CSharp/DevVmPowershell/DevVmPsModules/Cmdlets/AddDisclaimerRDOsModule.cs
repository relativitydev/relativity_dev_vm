﻿using Helpers.Implementations;
using Helpers.Interfaces;
using System;
using System.Management.Automation;
using System.Threading;

namespace DevVmPsModules.Cmdlets
{
	[Cmdlet(VerbsCommon.Add, "DisclaimerRDOs")]
	public class AddDisclaimerRDOsModule : BaseModule
	{
		[Parameter(
			Mandatory = true,
			ValueFromPipelineByPropertyName = true,
			ValueFromPipeline = true,
			Position = 0,
			HelpMessage = "Name of the Relativity Instance")]
		public string RelativityInstanceName { get; set; }

		[Parameter(
			Mandatory = true,
			ValueFromPipelineByPropertyName = true,
			ValueFromPipeline = true,
			Position = 1,
			HelpMessage = "Username of the Relativity Admin")]
		public string RelativityAdminUserName { get; set; }

		[Parameter(
			Mandatory = true,
			ValueFromPipelineByPropertyName = true,
			ValueFromPipeline = true,
			Position = 2,
			HelpMessage = "Password of the Relativity Admin")]
		public string RelativityAdminPassword { get; set; }

		[Parameter(
			Mandatory = true,
			ValueFromPipelineByPropertyName = true,
			ValueFromPipeline = true,
			Position = 3,
			HelpMessage = "Username of the Relativity Sql Account")]
		public string SqlAdminUserName { get; set; }

		[Parameter(
			Mandatory = true,
			ValueFromPipelineByPropertyName = true,
			ValueFromPipeline = true,
			Position = 4,
			HelpMessage = "Password of the Relativity Sql Account")]
		public string SqlAdminPassword { get; set; }
		[Parameter(
			Mandatory = true,
			ValueFromPipelineByPropertyName = true,
			ValueFromPipeline = true,
			Position = 5,
			HelpMessage = "Workspace Name that you want to install the application in")]
		public string WorkspaceName { get; set; }

		protected override void ProcessRecordCode()
		{
			//Validate Input arguments
			ValidateInputArguments();

			IConnectionHelper connectionHelper = new ConnectionHelper(
				relativityInstanceName: RelativityInstanceName,
				relativityAdminUserName: RelativityAdminUserName,
				relativityAdminPassword: RelativityAdminPassword,
				sqlAdminUserName: SqlAdminUserName,
				sqlAdminPassword: SqlAdminPassword);
			IDisclaimerAcceptanceHelper disclaimerAcceptanceHelper = new DisclaimerAcceptanceHelper(connectionHelper);

			// Add Disclaimer Configuration and Disclaimer
			Thread.Sleep(15000);
			disclaimerAcceptanceHelper.AddDisclaimerConfiguration(WorkspaceName);
			Thread.Sleep(15000);
			disclaimerAcceptanceHelper.AddDisclaimer(WorkspaceName);
		}

		private void ValidateInputArguments()
		{
			if (string.IsNullOrWhiteSpace(RelativityInstanceName))
			{
				throw new ArgumentNullException(nameof(RelativityInstanceName), $"{nameof(RelativityInstanceName)} cannot be NULL or Empty.");
			}

			if (string.IsNullOrWhiteSpace(RelativityAdminUserName))
			{
				throw new ArgumentNullException(nameof(RelativityAdminUserName), $"{nameof(RelativityAdminUserName)} cannot be NULL or Empty.");
			}

			if (string.IsNullOrWhiteSpace(RelativityAdminPassword))
			{
				throw new ArgumentNullException(nameof(RelativityAdminPassword), $"{nameof(RelativityAdminPassword)} cannot be NULL or Empty.");
			}

			if (string.IsNullOrWhiteSpace(WorkspaceName))
			{
				throw new ArgumentNullException(nameof(WorkspaceName), $"{nameof(WorkspaceName)} cannot be NULL or Empty.");
			}
		}
	}
}
