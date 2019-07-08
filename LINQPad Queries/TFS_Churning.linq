<Query Kind="Program">
  <Reference>&lt;ProgramFilesX86&gt;\Microsoft Visual Studio\2017\Community\Common7\IDE\CommonExtensions\Microsoft\TeamFoundation\Team Explorer\Microsoft.TeamFoundation.Client.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\Microsoft Visual Studio\2017\Community\Common7\IDE\CommonExtensions\Microsoft\TeamFoundation\Team Explorer\Microsoft.TeamFoundation.Common.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\Microsoft Visual Studio\2017\Community\Common7\IDE\CommonExtensions\Microsoft\TeamFoundation\Team Explorer\Microsoft.TeamFoundation.Controls.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\Microsoft Visual Studio\2017\Community\Common7\IDE\CommonExtensions\Microsoft\TeamFoundation\Team Explorer\Microsoft.TeamFoundation.VersionControl.Client.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\Microsoft Visual Studio\2017\Community\Common7\IDE\CommonExtensions\Microsoft\TeamFoundation\Team Explorer\Microsoft.TeamFoundation.VersionControl.Common.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\Microsoft Visual Studio\2017\Community\Common7\IDE\CommonExtensions\Microsoft\TeamFoundation\Team Explorer\Microsoft.TeamFoundation.VersionControl.Common.Integration.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\Microsoft Visual Studio\2017\Community\Common7\IDE\CommonExtensions\Microsoft\TeamFoundation\Team Explorer\Microsoft.TeamFoundation.VersionControl.ControlAdapter.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\Microsoft Visual Studio\2017\Community\Common7\IDE\CommonExtensions\Microsoft\TeamFoundation\Team Explorer\Microsoft.TeamFoundation.VersionControl.Controls.Common.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\Microsoft Visual Studio\2017\Community\Common7\IDE\CommonExtensions\Microsoft\TeamFoundation\Team Explorer\Microsoft.TeamFoundation.VersionControl.Controls.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\Microsoft Visual Studio\2017\Community\Common7\IDE\CommonExtensions\Microsoft\TeamFoundation\Team Explorer\Microsoft.TeamFoundation.VersionControl.UIFeatures.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\Microsoft Visual Studio\2017\Community\Common7\IDE\CommonExtensions\Microsoft\TeamFoundation\Team Explorer\Microsoft.TeamFoundation.WorkItemTracking.Client.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\Microsoft Visual Studio\2017\Community\Common7\IDE\CommonExtensions\Microsoft\TeamFoundation\Team Explorer\Microsoft.TeamFoundation.WorkItemTracking.Common.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\Microsoft Visual Studio\2017\Community\Common7\IDE\CommonExtensions\Microsoft\TeamFoundation\Team Explorer\Microsoft.TeamFoundation.WorkItemTracking.Controls.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\Microsoft Visual Studio\2017\Community\Common7\IDE\CommonExtensions\Microsoft\TeamFoundation\Team Explorer\Microsoft.VisualStudio.Services.Client.Interactive.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\Microsoft Visual Studio\2017\Community\Common7\IDE\CommonExtensions\Microsoft\TeamFoundation\Team Explorer\Microsoft.VisualStudio.Services.Common.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\Microsoft Visual Studio\2017\Community\Common7\IDE\CommonExtensions\Microsoft\TeamFoundation\Team Explorer\Microsoft.VisualStudio.Services.Integration.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\Microsoft Visual Studio\2017\Community\Common7\IDE\CommonExtensions\Microsoft\TeamFoundation\Team Explorer\Microsoft.VisualStudio.Services.WebApi.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.dll</Reference>
  <Namespace>Microsoft.TeamFoundation</Namespace>
  <Namespace>Microsoft.TeamFoundation.Client</Namespace>
  <Namespace>Microsoft.TeamFoundation.Common</Namespace>
  <Namespace>Microsoft.TeamFoundation.Controls</Namespace>
  <Namespace>Microsoft.TeamFoundation.Framework.Client</Namespace>
  <Namespace>Microsoft.TeamFoundation.Server</Namespace>
  <Namespace>Microsoft.TeamFoundation.VersionControl.Client</Namespace>
  <Namespace>Microsoft.TeamFoundation.VersionControl.Common</Namespace>
  <Namespace>Microsoft.TeamFoundation.Warehouse</Namespace>
  <Namespace>Microsoft.TeamFoundation.WorkItemTracking.Client</Namespace>
  <Namespace>System.Net</Namespace>
</Query>

void Main()
{

	var path = "$/Blink.Arpa";
	//var path = "$/";
	var tfsUrl = new Uri("https://tfs.blink.hr/BlinkCollection");
	// TeamFoundationServer tfs = new TeamFoundationServer(tfsUrl);
	var server = new TfsTeamProjectCollection(tfsUrl, CredentialCache.DefaultNetworkCredentials);
	var _workItemStore = server.GetService<WorkItemStore>();
	var vcs = server.GetService<VersionControlServer>();
	var fromVersion = GetDateVSpec(DateTime.Now.AddMonths(-12));
	var toVersion = VersionSpec.Latest;
	// var _project = _workItemStore.Projects.Cast<Project>().Where(p => p.Name == "Blink.Arpa").FirstOrDefault().Dump();

	// IEnumerable changesets = vcs.QueryHistory(path, VersionSpec.Latest, 0, RecursionType.Full, null, null, LatestVersionSpec.Latest, 10, true, false);

	var res = vcs.QueryHistory(path, VersionSpec.Latest, 0, RecursionType.Full, null, fromVersion, toVersion, int.MaxValue, false, false, false);
	var takenChanges = res.OfType<Changeset>();
	//takenChanges.Select(GetChangesetWithChanges).Dump();


	GetUserStatistics().Dump();
	GetUserStatisticsRaw().Dump();
	//GetChurnStatistics().Dump();
	//GetChurnStatisticsRaw().Dump();



	//	takenChanges.Select(GetChangesetWithChanges)
	//	            //.SelectMany(d => d.WorkItems)
	//                .SelectMany(c => c.Changes.Select(d => d.Item)) // select the actual changed files
	//                .Where(c => c.Item.ServerItem.Contains("/Blink.Arpa/")) // filter out just the files we are interested in
	//                .Where(c => c.Item.ServerItem.EndsWith(".cs") || c.Item.ServerItem.EndsWith(".js"))
	//                .Where(c => ((int)c.ChangeType & (int)Microsoft.TeamFoundation.VersionControl.Client.ChangeType.Edit) == (int)Microsoft.TeamFoundation.VersionControl.Client.ChangeType.Edit) // don't count merges
	//                //.Select(c => Regex.Replace(c.Item.ServerItem, @"^.+/Blink.Arpa/", "")) // count changes to the same file on different branches
	//                .Select(cc => new { File = Regex.Replace(cc.Item.ServerItem, @"^.+/Blink.Arpa/", ""),
	////			        User = cc.Item.ServerItem.Commiter, 
	////			        LinesAdded = cc.LinesAdded,
	////			        LinesDeleted = cc.LinesDeleted,
	////			        LinesModifided = cc.LinesModified
	//    			})
	//				.GroupBy(c => c)
	//				.ToList().Dump();

	//    foreach (Changeset cSet in changesets) 
	//    { 
	//	  if(cSet.Changes.Length > 0 )
	//	  {
	//		  Console.WriteLine(string.Format("{0} - {1} - {2} - {3}", cSet.ChangesetId, cSet.Committer, cSet.CreationDate, cSet.Comment)); 
	//		  foreach (Change change in cSet.Changes)
	//		  {
	//			  Console.WriteLine( string.Format("  {0}  {1}", change.ChangeType, change.Item.ServerItem));						
	//		  }
	//	  }
	//}

	Changeset GetChangesetWithChanges(Changeset c)
	{
		return vcs.GetChangeset(c.ChangesetId, includeChanges: true, includeDownloadInfo: true);
	}

	IEnumerable<SourceControlStatistic> GetUserStatistics()
	{
		return takenChanges.GroupBy(c => c.Committer)
					.Select(g => new SourceControlStatistic { Key = g.Key, Count = g.Count() }).OrderByDescending(s => s.Count);
	}

	IEnumerable GetUserStatisticsRaw()
	{
		return takenChanges.Select(GetChangesetWithChanges).Select(x => new { Ches = x.Changes, Changeset = x })
			   .GroupBy(c => new { MyChanges = c.Ches, MyChangesetCommitter = c.Changeset.Committer }, (g, r) => new
			   {
				   Committer = g.MyChangesetCommitter,
				   MyChanges = g.MyChanges
							.Where(c => c.Item.ServerItem.EndsWith(".cs") || c.Item.ServerItem.EndsWith(".js") || c.Item.ServerItem.EndsWith(".ts") || c.Item.ServerItem.EndsWith(".html") || c.Item.ServerItem.EndsWith(".cshtml"))
							.Where(oj => oj.ChangeType == Microsoft.TeamFoundation.VersionControl.Client.ChangeType.Edit),

			   })
								.Where(gr => gr.MyChanges.Count() > 0)
			  .Select(g => new SourceControlStatistic { Key = g.Committer, Count = g.MyChanges.Count() })
			  .GroupBy(gg => gg.Key)
			  .Select(g => new SourceControlStatistic { Key = g.Key, Count = g.Count() })
			  .OrderByDescending(s => s.Count);
		;

	}


	VersionSpec GetDateVSpec(DateTime date)
	{
		string dateSpec = string.Format("D{0:yyy}-{0:MM}-{0:dd}T{0:HH}:{0:mm}", date);
		return VersionSpec.ParseSingleSpec(dateSpec, "");
	}

	IEnumerable GetChurnStatisticsRaw()
	{
		return takenChanges
			.Select(GetChangesetWithChanges)
			.SelectMany(c => c.Changes) // select the actual changed files 
			.Where(c => c.Item.ServerItem.EndsWith(".cs") || c.Item.ServerItem.EndsWith(".js") || c.Item.ServerItem.EndsWith(".ts") || c.Item.ServerItem.EndsWith(".html") || c.Item.ServerItem.EndsWith(".cshtml"))
			.Where(c => (c.ChangeType == Microsoft.TeamFoundation.VersionControl.Client.ChangeType.Edit))
		;
	}

	IEnumerable<SourceControlStatistic> GetChurnStatistics()
	{
		return takenChanges
			.Select(GetChangesetWithChanges)
			.SelectMany(c => c.Changes) // select the actual changed files 
										//.Where(c => c.Item.ServerItem.Contains("/Blink.Arpa/")) // filter out just the files we are interested in 

			.Where(c => c.Item.ServerItem.EndsWith(".cs") || c.Item.ServerItem.EndsWith(".js") || c.Item.ServerItem.EndsWith(".ts") || c.Item.ServerItem.EndsWith(".html") || c.Item.ServerItem.EndsWith(".cshtml"))
			//.Where(c => ((int) c.ChangeType & (int) Microsoft.TeamFoundation.VersionControl.Client.ChangeType.Edit) == (int) Microsoft.TeamFoundation.VersionControl.Client.ChangeType.Edit)
			.Where(c => c.ChangeType == Microsoft.TeamFoundation.VersionControl.Client.ChangeType.Edit)
			// don't count merges 
			.Select(c => Regex.Replace(c.Item.ServerItem, @"^.+/Arpa_Revizije/", ""))
			.Select(c => Regex.Replace(c, @"^.+/Blink.EuMobil/", ""))
			.Select(c => Regex.Replace(c, @"^.+/BEL/", ""))
			.Select(c => Regex.Replace(c, @"^.+/BLL/", ""))
			.Select(c => Regex.Replace(c, @"^.+/DAL/", ""))
			.Select(c => c)
			// count changes to the same file on different branches 
			.GroupBy(c => c)
			.Select(g =>
				new SourceControlStatistic { Key = g.Key, Count = g.Count() }).OrderByDescending(s => s.Count);
	}
}

class SourceControlStatistic
{
	public string Key { get; set; }
	public int Count { get; set; }
}

//void Main()
//{
//    var churns = FactCodeChurns
//        .Where (cc => cc.FilenameSKDimFile.FileExtension == ".cs")
//    .Select(cc => new { File = Regex.Replace(cc.FilenameSKDimFile.FilePath , @"^.+/Source( Code)?/", ""),
//        User = cc.DimChangeset.CheckedInBySKDimPerson.Name, 
//        LinesAdded = cc.LinesAdded,
//        LinesDeleted = cc.LinesDeleted,
//        LinesModifided = cc.LinesModified
//    });
//    
//    var changes = new Dictionary<string, Stats>();
//    var users = new Dictionary<string, HashSet<string>>();
//    foreach(var churn in churns)
//    {
//        Stats stats;
//        if(!changes.TryGetValue(churn.File, out stats))
//        {
//            changes[churn.File] = stats = new Stats();
//        }
//        stats.Usages++;
//        stats.LinesAdded += churn.LinesAdded.Value;
//        stats.LinesDeleted += churn.LinesDeleted.Value;
//        stats.LinesModified += churn.LinesModifided.Value;
//        
//        HashSet<string> fileUsers;
//        if(!users.TryGetValue(churn.File, out fileUsers))
//        {
//            users[churn.File] = fileUsers = new HashSet<string>();
//        }
//        fileUsers.Add(churn.User);
//    }
//    
//    changes.Where(kvp => kvp.Value.Usages > 200)
//            .OrderByDescending(kvp => kvp.Value.Usages)
//            .Select(kvp => new { kvp.Key, kvp.Value.Usages })
//            .Dump("Usages");
//    changes.Where(kvp => kvp.Value.LinesModified > 5000)
//            .OrderByDescending(kvp => kvp.Value.LinesModified)
//            .Select(kvp => new { kvp.Key, kvp.Value.LinesModified })
//            .Dump("Lines Modified");
//    changes.Where(kvp => kvp.Value.LinesAdded > 50000)
//            .OrderByDescending(kvp => kvp.Value.LinesAdded)
//            .Select(kvp => new { kvp.Key, kvp.Value.LinesAdded })
//            .Dump("Lines Added");
//    changes.Where(kvp => kvp.Value.LinesDeleted > 40000)
//            .OrderByDescending(kvp => kvp.Value.LinesDeleted)
//            .Select(kvp => new { kvp.Key, kvp.Value.LinesDeleted })
//            .Dump("Lines Deleted");
//            
//    users.Where(kvp => kvp.Value.Count > 20)
//        .OrderByDescending (kvp => kvp.Value.Count)
//        .Select(kvp => new { kvp.Key, kvp.Value })
//        .Dump("Users");    
//
//}
//
//class Stats
//{
//    public int Usages { get; set; }
//    public int LinesAdded { get; set; }
//    public int LinesDeleted { get; set; }
//    public int LinesModified { get; set; }
//}
//