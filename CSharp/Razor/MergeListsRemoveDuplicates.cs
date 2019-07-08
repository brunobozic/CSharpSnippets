public List<ListOfAssesmentCriteriaDTO> GetAssesmentCriteriaFilteredByKeyRequirements( int? auditId , List<int> listOfSelectedKRs )
        {
            var ctx = base.Context;

            // create a list of items assesment criteria that are related to the provided list of selected key requirements
            var items1 =
            ( from ar in ctx.Audits
              join aop in ctx.AuditOperationalProgramme on ar.Id equals aop.AuditId
              join ad in ctx.AuditDetail on aop.Id equals ad.AuditOperationalProgrammeId
              join clsa in ctx.ChecklistSysaudit on ad.Id equals clsa.AuditDetailId
              join ac in ctx.AssesmentCriteria on clsa.AssesmentCriteriaId equals ac.Id
              join kr in ctx.KeyRequirement on ac.KeyRequirementId equals kr.Id
              where ar.Id == auditId && listOfSelectedKRs.Contains(( int ) ac.KeyRequirementId)
              orderby ac.Id
              select new ListOfAssesmentCriteriaDTO
              {
                  Id = ac.Id ,
                  Label = ac.Label ,
                  Selected = true ,
                  Disabled = true
              } ).AsNoTracking().ToList();

            // get a list of assesment criteria items that are not *directly* related to the provided list of selected key requirements, but
            // that still need to be added to the dropdown list so the user can select them
            var items2 =
            ( from ac in ctx.AssesmentCriteria
              join kr in ctx.KeyRequirement on ac.KeyRequirementId equals kr.Id
              where listOfSelectedKRs.Contains(( int ) ac.KeyRequirementId)
              orderby ac.Id
              select new ListOfAssesmentCriteriaDTO
              {
                  Id = ac.Id ,
                  Label = ac.Label
              } ).AsNoTracking().ToList();

            // merge lists
            items1.AddRange(items2);

            // remove duplicates
            items1.RemoveAll(x => items2.Exists(y => y.Id == x.Id));

            return items2;}