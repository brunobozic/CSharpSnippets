<Query Kind="Statements">
  <Connection>
    <ID>e2bac635-3f81-4afb-b14a-bb5759b6f770</ID>
    <Persist>true</Persist>
    <Driver>EntityFrameworkDbContext</Driver>
    <CustomAssemblyPath>C:\Workspac\Blink.Arpa\Dev-Feat-FinalReport-Findings-Rework\trunk\BEL\bin\Debug\BEL.dll</CustomAssemblyPath>
    <CustomTypeName>BEL.ArpaEntities</CustomTypeName>
    <AppConfigPath>C:\Workspac\Blink.Arpa\Dev-Feat-FinalReport-Findings-Rework\trunk\Arpa_Revizije\Web.config</AppConfigPath>
  </Connection>
  <Output>DataGrids</Output>
  <Reference>C:\Workspac\Blink.Arpa\Dev-Feat-FinalReport-Findings-Rework\trunk\Arpa_Revizije\bin\BEL.dll</Reference>
  <Reference>C:\Workspac\Blink.Arpa\Dev-Feat-FinalReport-Findings-Rework\trunk\Arpa_Revizije\bin\BLL.dll</Reference>
  <Reference>C:\Workspac\Blink.Arpa\Dev-Feat-FinalReport-Findings-Rework\trunk\Arpa_Revizije\bin\DAL.dll</Reference>
  <Reference>C:\Workspac\Blink.Arpa\Dev-Feat-FinalReport-Findings-Rework\trunk\Arpa_Revizije\bin\EntityFramework.dll</Reference>
  <Reference>C:\Workspac\Blink.Arpa\Dev-Feat-FinalReport-Findings-Rework\trunk\Arpa_Revizije\bin\Microsoft.ApplicationInsights.TraceListener.dll</Reference>
  <Namespace>BEL</Namespace>
  <Namespace>BEL.Model</Namespace>
  <Namespace>BLL</Namespace>
  <Namespace>BLL.Model</Namespace>
  <Namespace>DAL.Common</Namespace>
  <Namespace>DAL.DTO</Namespace>
  <Namespace>DAL.Repository</Namespace>
</Query>

var auditId = 1003;
var sampleItemId = 2006;


 var retval = (
                   from find in Finding  
				   join clopa in ChecklistOperationAnswer on find.ChecklistOperationAnswerId equals clopa.Id
				   join clopq in ChecklistOperationQuestion on clopa.QuestionId equals clopq.Id
				   join clop in ChecklistOperation on clopq.ChecklistOperationId equals clop.Id
                   join sampledItem in ParametersOfSampledItem on clop.ParametersOfSampledItemId equals sampledItem.Id
                   join audit in Audits on sampledItem.AuditId equals audit.Id
				   //join findingStatus in FindingStatus on find.Id equals findingStatus.FindingId into
                   //    findingStatusLeftJoin
                   //from findingStatusLj in findingStatusLeftJoin.DefaultIfEmpty()
                   join findingStatus in FindingStatus on find.Id equals findingStatus.FindingId
                   //join statusFind in StatusFinding on findingStatusLj.StatusFindingId equals statusFind.Id into
                   //    statusFindingLeftJoin
                   //from statusFindLj in statusFindingLeftJoin.DefaultIfEmpty()
                   join statusFind in StatusFinding on findingStatus.StatusFindingId equals statusFind.Id
                   join statusGeneralFind in StatusGeneralFindings on statusFind.StatusGeneralFindingId equals
                       statusGeneralFind.Id into statusGeneralFindLeftJoin
				   from statusGeneralFindLj in statusGeneralFindLeftJoin.DefaultIfEmpty()
				   
                   join drsiWrapper in DraftReportSampledItemWrappers.DefaultIfEmpty() on
                       sampledItem.DraftReportSampledItemWrapperId equals drsiWrapper.DraftReportSampledItemWrapperId
                       into draftReportSampledItemWrapperLeftJoin
                   from draftReportSampledItemWrapperLj in draftReportSampledItemWrapperLeftJoin.DefaultIfEmpty()
                   join draftReport in DraftReportSampledItems.DefaultIfEmpty() on draftReportSampledItemWrapperLj
                       .DraftReportSampledItemWrapperId equals draftReport.DraftReportSampledItemWrappers
                       .DraftReportSampledItemWrapperId into draftReportSampledItemLeftJoin
                   from draftReportSampledItemLj in draftReportSampledItemLeftJoin.DefaultIfEmpty().
                   join finalReportWrapper in FinalReportSampledItemWrappers.DefaultIfEmpty() on
                       sampledItem.FinalReportSampledItemWrapperId equals finalReportWrapper
                           .FinalReportSampledItemWrapperId into finalReportSampledItemWrapperLeftJoin
                   from finalReportSampledItemWrapperLeftJoinLj in
                       finalReportSampledItemWrapperLeftJoin.DefaultIfEmpty()
                   join finalReport in FinalReportSampledItems.DefaultIfEmpty() on
                       finalReportSampledItemWrapperLeftJoinLj.FinalReportSampledItemWrapperId equals finalReport
                           .FinalReportSampledItemWrapperId into finalReportSampledItemLeftJoin
                   from finalReportSampledItemLj in finalReportSampledItemLeftJoin.DefaultIfEmpty()
				   where audit.Id == auditId 
						   && !find.IsDeleted
						   && find.TypeOfFinding == 0
						   && statusFind.Id == 5
						&& sampledItem.Id == sampleItemId // basic set filtering
                   group findingStatus by new
                   {
                       MyFindings = find,
					   MySampledItems= sampledItem
	                }
                   into groupedByFindingStatus // grouping by finding status
                   let stuff = groupedByFindingStatus.OrderByDescending(g => g.date) // ordering parameters of sampled items statuses by date
                   select new // projection into an anonymous type whilst obeying the previous grouping and ordering
                   {
                       groupedByFindingStatus.Key.MyFindings,
					   MySampledItems = groupedByFindingStatus.Key.MySampledItems,
                       FIlteredFindingStatuses = stuff.FirstOrDefault(), // taking top one newest finding status, after having ordered it by date descending
                       MyStatusFinding = stuff.FirstOrDefault().StatusFinding,
                       MyStatusGeneralFinding = stuff.FirstOrDefault().StatusFinding.StatusGeneralFindings,
                     //  MyDraftReportWrapper = groupedByFindingStatus.Key.MySampledItems.DraftReportSampledItemWrappers,
                     //  MyFinalReportWrapper = groupedByFindingStatus.Key.MySampledItems.FinalReportSampledItemWrappers,
					 //  MyFindings = groupedByFindingStatus.Key.MySampledItems.ChecklistOperation.SelectMany(i=>i.ChecklistOperationQuestion.SelectMany(iu=>iu.ChecklistOperationAnswer.SelectMany(io=>io.Finding)))
				   })
				   .Select(anon => new ParametersOfSampledItemsDTO                                 // projecting into a DTO and constructing a proper object graph
                   {
                       IsDeleted = anon.MySampledItems.IsDeleted,
                       Id = (int?)anon.MySampledItems.Id,
                       AuditId = (int?)anon.MySampledItems.Audits.Id,
                       OperationTitle = anon.MySampledItems.OperationalTitle,
                       OperationCode = anon.MySampledItems.OperationalCode,
                       BeneficieryTitle = anon.MySampledItems.FinalBeneficiaryTitle,
                       LocationOfBeneficiary = anon.MySampledItems.FinalBeneficiaryCity,
                       OperationalProgramme = anon.MySampledItems.OperationalTitle,
                       PriorityAxis = anon.MySampledItems.PriorityLabel,
                       AuditedAmount = (decimal?)anon.MySampledItems.AuditedAmount,
                       AuditedAmountPercent = (decimal?)anon.MySampledItems.AuditedAmountPercent,
                       DeclaredAmount = (decimal?)anon.MySampledItems.BookValue,
                       AuditStaredAt = anon.MySampledItems.StartDateOfAudit,
                       AuditEndedAt = anon.MySampledItems.EndDateOfAudit,
                       BookValue = (decimal?)anon.MySampledItems.BookValue,
                       AuditStatusFullName = anon.MySampledItems.Audits.AuditStatus.FullName,
                       AuditStatusId = (int?)anon.MySampledItems.Audits.AuditStatusId,
                       AuditTitle = anon.MySampledItems.Audits.AuditTitle,
                       PeriodId = (int?)anon.MySampledItems.PeriodId,
                       ProjectName = anon.MySampledItems.ProjectName,
                       Finding = new FindingDTO
                       {
                           IsReadOnly = anon.MyFindings.IsReadOnly,
                           IsDeleted = anon.MyFindings.IsDeleted,
                           Title = anon.MyFindings.Title,
                           Id = anon.MyFindings.Id,
                           Label = anon.MyFindings.Label,
                           TypeOfFinding = anon.MyFindings.TypeOfFinding,
                           ReadOnlyUtc = anon.MyFindings.ReadOnlyUtc,
                           ReadOnlyReason = anon.MyFindings.ReadOnlyReason,
                           Comment = anon.MyFindings.Comment,
                           OriginatingFinding = anon.MyFindings.OriginatingFinding,
                           DateModified = anon.MyFindings.DateModified,
                           ModifiedBy = anon.MyFindings.ModifiedBy,
                           DateCreated = anon.MyFindings.DateCreated,
                           DateDeleted = anon.MyFindings.DateDeleted,
                           DeletedBy = anon.MyFindings.DeletedBy,
                           FindingStatus = anon.FIlteredFindingStatuses != null ? (new FindingStatusDTO
                           {
                               Id = (int?)anon.FIlteredFindingStatuses.Id,
                               date = (DateTime?)anon.FIlteredFindingStatuses.date,
                               StatusFindingId = (int?)anon.FIlteredFindingStatuses.StatusFindingId,
                               FindingId = (int?)anon.FIlteredFindingStatuses.FindingId,
                               StatusFinding = anon.MyStatusFinding != null ? (new StatusFindingDTO
                               {
                                   Id = (int?)anon.MyStatusFinding.Id,
                                   StatusShort = anon.MyStatusFinding.StatusShort,
                                   StatusLong = anon.MyStatusFinding.StatusLong,
                                   StatusGeneralFindingId = (int?)anon.MyStatusFinding.StatusGeneralFindingId,
                                   IsActive = (bool?)anon.MyStatusFinding.IsActive,
                                   StatusGeneralFinding = anon.MyStatusGeneralFinding != null ? (new StatusGeneralFindingDto
                                   {
                                       Id = (int?)anon.MyStatusGeneralFinding.Id,
                                       Name = anon.MyStatusGeneralFinding.Name
                                   }) : null
                               }) : null
                           }) : null
                       }
//					   ,
//                       DraftReportSampledItemWrappers = anon.MyDraftReportWrapper.Select(x => new DraftReportSampledItemWrapperDTO
//                       {
//                           DraftReportSampledItemWrapperId = (int?)x.DraftReportSampledItemWrapperId,
//                           IsDeleted = (bool?)x.IsDeleted,
//                           LastModified = x.LastModified,
//                           LastModifiedBy = x.LastModifiedBy,
//                           CreatedBy = x.CreatedBy,
//                           DateCreated = x.DateCreated,
//                           DateDeleted = x.DateDeleted,
//                           DeletedBy = x.DeletedBy,
//                           AuditId = (int?)x.ParametersOfSampledItem.Audits.Id,
//                           ParameterOfSampledItemId = (int?)x.DraftReportSampledItemWrapperId,
//                           DraftReports = x.DraftReportSampledItems.Select(b => new DraftReportSampledItemDTO
//                           {
//                               IsDeleted = (bool?)b.IsDeleted,
//                               DraftReportSampledItemId = (int?)b.ParametersOfSampledItem.Id,
//                               Status = (int?)b.Status,
//                               THistoryIdentifier = b.THistoryIdentifier,
//                               LastModified = b.LastModified,
//                               LastModifiedBy = b.LastModifiedBy,
//                               DateCreated = b.DateCreated,
//                               DateDeleted = b.DateDeleted,
//                               DeletedBy = b.DeletedBy,
//                               ParametersOfSampledItemId = b.ParametersOfSampledItem.Id
//                           }).ToList()
//                       }).ToList()
//					   ,
//                       FinalReportSampledItemWrappers = anon.MyFinalReportWrapper.Select(x => new FinalReportSampledItemWrapperDTO
//                       {
//                           FinalReportSampledItemWrapperId = x.FinalReportSampledItemWrapperId,
//                           IsDeleted = x.IsDeleted,
//                           LastModified = x.LastModified,
//                           LastModifiedBy = x.LastModifiedBy,
//                           CreatedBy = x.CreatedBy,
//                           DateCreated = x.DateCreated,
//                           DateDeleted = x.DateDeleted,
//                           DeletedBy = x.DeletedBy,
//                           AuditId = x.ParametersOfSampledItem.Audits.Id,
//                           ParameterOfSampledItemId = x.ParametersOfSampledItemId,
//                           FinalReports = x.FinalReportSampledItems.Select(b => new FinalReportSampledItemDTO
//                           {
//                               IsDeleted = b.IsDeleted,
//                               FinalReportSampledItemId = b.ParametersOfSampledItem.Id,
//                               Status = b.Status,
//                               THistoryIdentifier = b.THistoryIdentifier,
//                               LastModified = b.LastModified,
//                               LastModifiedBy = b.LastModifiedBy,
//                               DateCreated = b.DateCreated,
//                               DateDeleted = b.DateDeleted,
//                               DeletedBy = b.DeletedBy,
//                               ParametersOfSampledItemId = b.ParametersOfSampledItem.Id
//                           }).ToList()
//					   }).ToList()
				   }).ToList().GroupBy(x=>x);

var sentinelValue = retval.Dump();