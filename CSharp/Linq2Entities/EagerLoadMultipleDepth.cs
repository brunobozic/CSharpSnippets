 var audit2 = ctx.Audits
                    .Include(i => i.AuditOperationalProgramme
                        .Select(j => j.AuditDetail
                            .Select(a => a.ChecklistSysaudit
                                .Select(k => k.AssesmentCriteria)
                                .Select(l => l.SysauditQuestion))));

                audit2 = audit2
                    .Include(i => i.AuditOperationalProgramme
                        .Select(j => j.AuditDetail.Select(k => k.Fond)));
                audit2 = audit2
                    .Include(i => i.AuditOperationalProgramme
                        .Select(j => j.AuditDetail.Select(k => k.Auditee)));

                var auditResult2 = audit2.SingleOrDefault(i => i.Id == auditId);

                var auditTypeId = auditResult2.AuditTypeId;
                var existingACs =
                    auditResult2.AuditOperationalProgramme
                        .Select(i => i.AuditDetail
                            .Select(k => k.ChecklistSysaudit
                                .Select(l => l.AssesmentCriteria)))
                        .FirstOrDefault()
                        .FirstOrDefault()
                        .ToList();