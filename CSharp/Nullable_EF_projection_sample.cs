// when you have a projection that features a nested "to one" relationship, that also has a sub-nested m2m relationship, you 
// need to force cast to nullable, and use a ternary expression like this:

            var query = Context.ParametersOfSampledItem
              .AsNoTracking()
              .Include(inc => inc.DraftReportSampledItemWrapper.DraftReportSampledItem)
              .Include(inc => inc.Audits)
              .Include(inc => inc.FinalReportSampledItemWrapper.FinalReportSampledItem)
              .Include(inc => inc.DraftReportSampledItemWrapper.ApprovalOfDocument)
              .Include(inc => inc.FinalReportSampledItemWrapper.ApprovalOfDocument)
              ;

            returnValue = query.Where(i => !i.IsDeleted && i.Audits.Id == auditId).Select(proj => new ParametersOfSampledItemsMirzaDTO
            {
                IsDeleted = proj.IsDeleted ,
                Id = proj.Id ,
                HasDraftReport = proj.HasDraftReport ,
                HasFinalReport = proj.HasFinalReport ,
                AuditId = proj.Audits.Id ,
                OperationTitle = proj.OperationalTitle ,
                OperationCode = proj.OperationalCode ,
                BeneficieryTitle = proj.FinalBeneficiaryTitle ,
                LocationOfBeneficiary = proj.FinalBeneficiaryCity ,
                OperationalProgramme = proj.OperationalTitle ,
                PriorityAxis = proj.PriorityLabel ,
                AuditedAmount = proj.AuditedAmount ,
                AuditedAmountPercent = proj.AuditedAmountPercent ,
                DeclaredAmount = proj.BookValue , // TODO: no idea where this property should come from...perhaps this should be "book value"
                AuditStaredAt = proj.StartDateOfAudit ,
                AuditEndedAt = proj.EndDateOfAudit ,
                BookValue = proj.BookValue ,

                DraftReportSampledItemWrapper = proj.DraftReportSampledItemWrapper != null ? new DraftReportSampledItemWrapperDTO 
                {
                    DraftReportSampledItemWrapperId = proj.DraftReportSampledItemWrapper.DraftReportSampledItemWrapperId ,
                    IsDeleted = proj.DraftReportSampledItemWrapper.IsDeleted ,
                    LastModified = proj.DraftReportSampledItemWrapper.LastModified ,
                    LastModifiedBy = proj.DraftReportSampledItemWrapper.LastModifiedBy ,
                    Status = (int?)proj.DraftReportSampledItemWrapper.Status??null ,
                    CreatedBy = proj.DraftReportSampledItemWrapper.CreatedBy ,
                    DateCreated = proj.DraftReportSampledItemWrapper.DateCreated ,
                    DateDeleted = proj.DraftReportSampledItemWrapper.DateDeleted ,
                    DeletedBy = proj.DraftReportSampledItemWrapper.DeletedBy ,
                    AuditId = proj.DraftReportSampledItemWrapper.AuditId ,
                    THistoryIdentifier = proj.DraftReportSampledItemWrapper.THistoryIdentifier ,
                    DraftReportSampledItem = proj.DraftReportSampledItemWrapper.DraftReportSampledItem.Select(o => new DraftReportSampledItemDTO
                    {
                        DraftReportSampledItemId = o.ParametersOfSampledItemId ,
                        IsDeleted = o.IsDeleted ,
                        Status = (int?) o.Status??null ,
                        THistoryIdentifier = o.THistoryIdentifier.Value ,
                        LastModified = o.DraftReportSampledItemWrapper.LastModified ,
                        LastModifiedBy = o.DraftReportSampledItemWrapper.LastModifiedBy ,
                        DateCreated = o.DraftReportSampledItemWrapper.DateCreated ,
                        DateDeleted = o.DraftReportSampledItemWrapper.DateDeleted ,
                        DeletedBy = o.DraftReportSampledItemWrapper.DeletedBy ,
                        ParametersOfSampledItemId = o.ParametersOfSampledItem.Id
                    })
                } : null
            } ).ToList();
