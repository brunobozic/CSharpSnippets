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
var sampledItemId = 2006;

var queryPosi = ParametersOfSampledItem
		.AsNoTracking()
		.Include(inc => inc.FinalReportSampledItems.Select(i => i.Audits))
		.Include(inc => inc.FinalReportSampledItemWrappers.Select(i => i.FinalReportSampledItems))
		.Include(inc => inc.DraftReportSampledItems.Select(i => i.Audits))
		.Include(inc => inc.DraftReportSampledItemWrappers.Select(i => i.DraftReportSampledItems))
		.Include(inc => inc.DraftReportSampledItemWrappers.Select(i => i.DraftReportSampledItems))
		.Include(inc => inc.ChecklistOperation.Select(au => au.ChecklistOperationQuestion.Select(ua => ua.ChecklistOperationAnswer.Select(eu => eu.Finding))))
	;

queryPosi = sampledItemId <= 0 ? queryPosi.Where(i => !i.IsDeleted && i.AuditId == auditId) : queryPosi.Where(i => !i.IsDeleted && i.AuditId == auditId && i.Id == sampledItemId);

var intermediateCollection = queryPosi.ToList();
var affectedFinding = intermediateCollection.SelectMany(aff => aff.ChecklistOperation.SelectMany(au => au.ChecklistOperationQuestion.SelectMany(ua => ua.ChecklistOperationAnswer.SelectMany(eu => eu.Finding)))).ToList();
var arrayOfFindingIds = affectedFinding.Select(findingIds => findingIds.Id).ToArray();

var findings = Finding
		.AsNoTracking()
		.Include(inc => inc.FindingStatus1.Select(i => i.StatusFinding.StatusGeneralFindings))
		.Where(f => arrayOfFindingIds.Contains(f.Id))
		.SelectMany(i => i.FindingStatus1)
		.GroupBy(group => group.date)
		.Select(g => g.OrderByDescending(group => group.date.Value)
		.FirstOrDefault())
;

var groupedFindings = findings.Select(ku => ku.Finding);

var fndingsDto = groupedFindings.Select(proj => new FindingDTO
{
	IsReadOnly = proj.IsReadOnly,
	IsDeleted = proj.IsDeleted,
	Title = proj.Title,
	Id = proj.Id,
	Label = proj.Label,
	TypeOfFinding = proj.TypeOfFinding,
	ReadOnlyUtc = proj.ReadOnlyUtc,
	ReadOnlyReason = proj.ReadOnlyReason,
	Comment = proj.Comment,
	OriginatingFinding = proj.OriginatingFinding,
	DateModified = proj.DateModified,
	ModifiedBy = proj.ModifiedBy,
	DateCreated = proj.DateCreated,
	DateDeleted = proj.DateDeleted,
	DeletedBy = proj.DeletedBy,
	FindingStatuses = proj.FindingStatus1.Select(joj => new FindingStatusDTO
	{
		Id = (int?)joj.Id,
		date = (DateTime?)joj.date,
		StatusFindingId = (int?)joj.StatusFindingId,
		FindingId = (int?)joj.FindingId,
		StatusFinding = joj.StatusFinding != null ? (new StatusFindingDTO
		{
			Id = (int?)joj.StatusFinding.Id,
			StatusShort = joj.StatusFinding.StatusShort,
			StatusLong = joj.StatusFinding.StatusLong,
			StatusGeneralFindingId = (int?)joj.StatusFinding.StatusGeneralFindingId,
			IsActive = (bool?)joj.StatusFinding.IsActive,
			StatusGeneralFinding = joj.StatusFinding.StatusGeneralFindings != null ? (new StatusGeneralFindingDto
			{
				Id = (int?)joj.StatusFinding.StatusGeneralFindings.Id,
				Name = joj.StatusFinding.StatusGeneralFindings.Name
			}) : null
		}) : null
	}).ToList()
});

var returnValue = queryPosi.Select(proj => new ParametersOfSampledItemsDTO
{
	IsDeleted = proj.IsDeleted,
	Id = (int?)proj.Id,
	AuditId = (int?)proj.Audits.Id,
	OperationTitle = proj.OperationalTitle,
	OperationCode = proj.OperationalCode,
	BeneficieryTitle = proj.FinalBeneficiaryTitle,
	LocationOfBeneficiary = proj.FinalBeneficiaryCity,
	OperationalProgramme = proj.OperationalTitle,
	PriorityAxis = proj.PriorityLabel,
	AuditedAmount = (decimal?)proj.AuditedAmount,
	AuditedAmountPercent = (decimal?)proj.AuditedAmountPercent,
	DeclaredAmount = (decimal?)proj.BookValue,
	AuditStaredAt = proj.StartDateOfAudit,
	AuditEndedAt = proj.EndDateOfAudit,
	BookValue = (decimal?)proj.BookValue,
	AuditStatusFullName = proj.Audits.AuditStatus.FullName,
	AuditStatusId = (int?)proj.Audits.AuditStatusId,
	AuditTitle = proj.Audits.AuditTitle,
	PeriodId = (int?)proj.PeriodId,
	ProjectName = proj.ProjectName,
	DraftReportSampledItemWrappers = proj.DraftReportSampledItemWrappers.Select(x => new DraftReportSampledItemWrapperDTO
	{
		DraftReportSampledItemWrapperId = (int?)x.DraftReportSampledItemWrapperId,
		IsDeleted = (bool?)x.IsDeleted,
		LastModified = x.LastModified,
		LastModifiedBy = x.LastModifiedBy,
		CreatedBy = x.CreatedBy,
		DateCreated = x.DateCreated,
		DateDeleted = x.DateDeleted,
		DeletedBy = x.DeletedBy,
		AuditId = (int?)x.ParametersOfSampledItem.Audits.Id,
		ParameterOfSampledItemId = (int?)x.DraftReportSampledItemWrapperId,
		DraftReports = x.DraftReportSampledItems.Select(b => new DraftReportSampledItemDTO
		{
			IsDeleted = (bool?)b.IsDeleted,
			DraftReportSampledItemId = (int?)b.ParametersOfSampledItem.Id,
			Status = (int?)b.Status,
			THistoryIdentifier = b.THistoryIdentifier,
			LastModified = b.LastModified,
			LastModifiedBy = b.LastModifiedBy,
			DateCreated = b.DateCreated,
			DateDeleted = b.DateDeleted,
			DeletedBy = b.DeletedBy,
			ParametersOfSampledItemId = b.ParametersOfSampledItem.Id
		}).ToList()
	}).ToList()
						   ,
	FinalReportSampledItemWrappers = proj.FinalReportSampledItemWrappers.Select(x => new FinalReportSampledItemWrapperDTO
	{
		FinalReportSampledItemWrapperId = x.FinalReportSampledItemWrapperId,
		IsDeleted = x.IsDeleted,
		LastModified = x.LastModified,
		LastModifiedBy = x.LastModifiedBy,
		CreatedBy = x.CreatedBy,
		DateCreated = x.DateCreated,
		DateDeleted = x.DateDeleted,
		DeletedBy = x.DeletedBy,
		AuditId = x.ParametersOfSampledItem.Audits.Id,
		ParameterOfSampledItemId = x.ParametersOfSampledItemId,
		FinalReports = x.FinalReportSampledItems.Select(b => new FinalReportSampledItemDTO
		{
			IsDeleted = b.IsDeleted,
			FinalReportSampledItemId = b.ParametersOfSampledItem.Id,
			Status = b.Status,
			THistoryIdentifier = b.THistoryIdentifier,
			LastModified = b.LastModified,
			LastModifiedBy = b.LastModifiedBy,
			DateCreated = b.DateCreated,
			DateDeleted = b.DateDeleted,
			DeletedBy = b.DeletedBy,
			ParametersOfSampledItemId = b.ParametersOfSampledItem.Id
		}).ToList()
	}).ToList()
}).ToList().Dump();