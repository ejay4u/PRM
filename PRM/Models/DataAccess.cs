using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using PRM.ViewModels;

namespace PRM.Models
{
    public class DataAccess
    {
        private MongoContext _dbContext;
        public DataAccess()
        {
            _dbContext = new MongoContext();
        }

        #region ClientTasks
        public async Task<IEnumerable<Client>> GetAllClients()
        {
            try
            {
                return await _dbContext.Clients.Find(_ => true).ToListAsync();
                //return await Mapper.Map<IEnumerable<BusinessDto>>(_dbContext.Businesses.Find(_ => true).ToListAsync()).Select(Mapper.Map<Business, BusinessDto>);
                //return await _dbContext.Businesses.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }

        }

        public async Task<Client> GetClient(string id)
        {
            var filter = Builders<Client>.Filter.Eq("Id", id);

            try
            {
                return await _dbContext.Clients
                                .Find(filter)
                                .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task AddClient(Client client)
        {
            client.ClientId = new AccountNoGen().RandomAccountNo();
            var builder = Builders<Client>.Filter;
            var filter = builder.Eq("ClientId", client.ClientId) & builder.Eq("Surname", client.Surname);
            var query = await _dbContext.Clients
                                .Find(filter)
                                .ToListAsync();
            try
            {
                if(query.Count <= 0)
                    await _dbContext.Clients.InsertOneAsync(client);
                else
                {

                }

            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> UpdateClient(ObjectId id, Client client)
        {
            try
            {
                ReplaceOneResult actionResult = await _dbContext.Clients
                                                .ReplaceOneAsync(n => n.Id.Equals(id)
                                                                , client
                                                                , new UpdateOptions { IsUpsert = true });
                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> RemoveClient(string id)
        {
            try
            {
                //Request Business Information
                var query = _dbContext.Clients.Find(Builders<Client>.Filter.Eq("_id", ObjectId.Parse(id))).ToEnumerable();
                if (query != null)
                {
                    var projection = Builders<LabRequest>.Projection.Include("ClientId");
                    var businesses = query as IList<Client> ?? query.ToList();
                    var campaings = _dbContext.LabRequests.Find(Builders<LabRequest>.Filter.Eq("ClientId", businesses.ElementAt(0).ClientId)).Project(projection).ToEnumerable();
                    if(campaings != null)
                    {
                        //Delete All CampaignCode For Campaigns By Business
                        foreach(var campaign in campaings)
                        {
                            await _dbContext.CampaignCodes.DeleteManyAsync(Builders<CampaignCode>.Filter.Eq("CampaignId", campaign.GetElement(0).Value.AsString));

                            //Delete Campaign Winners
                            await _dbContext.PassItOns.DeleteManyAsync(Builders<PassItOn>.Filter.Eq("CampaignId", campaign.GetElement(0).Value.AsString));

                            //Delete Card Generation
                            await _dbContext.CodeCard.DeleteManyAsync(Builders<CodeCard>.Filter.Eq("CampaignId", campaign.GetElement(0).Value.AsString));
                        }

                        //Delete All Campaigns By Business
                        await _dbContext.LabRequests.DeleteManyAsync(Builders<LabRequest>.Filter.Eq("ClientId", businesses.ElementAt(0).ClientId));
                    }
                    
                }

                //Delete the Business
                DeleteResult actionResult = await _dbContext.Clients.DeleteOneAsync(
                     Builders<Client>.Filter.Eq("_id", ObjectId.Parse(id)));

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> RemoveAllClients()
        {
            try
            {
                DeleteResult actionResult = await _dbContext.Clients.DeleteManyAsync(new BsonDocument());

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        #endregion

        #region DoctorTasks
        public async Task<IEnumerable<Doctor>> GetAllDoctors()
        {
            try
            {
                return await _dbContext.Doctors.Find(_ => true).ToListAsync();
                //return await Mapper.Map<IEnumerable<BusinessDto>>(_dbContext.Businesses.Find(_ => true).ToListAsync()).Select(Mapper.Map<Business, BusinessDto>);
                //return await _dbContext.Businesses.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }

        }

        public async Task<Doctor> GetDoctor(string id)
        {
            var filter = Builders<Doctor>.Filter.Eq("Id", id);

            try
            {
                return await _dbContext.Doctors
                                .Find(filter)
                                .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task AddDoctor(Doctor Doctor)
        {
            Doctor.DoctorId = new AccountNoGen().RandomAccountNo();
            var builder = Builders<Doctor>.Filter;
            var filter = builder.Eq("DoctorId", Doctor.DoctorId) & builder.Eq("Surname", Doctor.Surname);
            var query = await _dbContext.Doctors
                                .Find(filter)
                                .ToListAsync();
            try
            {
                if (query.Count <= 0)
                    await _dbContext.Doctors.InsertOneAsync(Doctor);
                else
                {

                }

            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> UpdateDoctor(ObjectId id, Doctor Doctor)
        {
            try
            {
                ReplaceOneResult actionResult = await _dbContext.Doctors
                                                .ReplaceOneAsync(n => n.Id.Equals(id)
                                                                , Doctor
                                                                , new UpdateOptions { IsUpsert = true });
                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> RemoveDoctor(string id)
        {
            try
            {
                //Delete the Business
                DeleteResult actionResult = await _dbContext.Doctors.DeleteOneAsync(
                     Builders<Doctor>.Filter.Eq("_id", ObjectId.Parse(id)));

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> RemoveAllDoctors()
        {
            try
            {
                DeleteResult actionResult = await _dbContext.Doctors.DeleteManyAsync(new BsonDocument());

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        #endregion

        #region ExaminationTasks
        public async Task<IEnumerable<Examination>> GetAllExaminations()
        {
            try
            {
                return await _dbContext.Examinations.Find(_ => true).ToListAsync();
                //return await Mapper.Map<IEnumerable<BusinessDto>>(_dbContext.Businesses.Find(_ => true).ToListAsync()).Select(Mapper.Map<Business, BusinessDto>);
                //return await _dbContext.Businesses.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }

        }

        public async Task<Examination> GetExamination(string id)
        {
            var filter = Builders<Examination>.Filter.Eq("Id", id);

            try
            {
                return await _dbContext.Examinations
                                .Find(filter)
                                .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task AddExamination(Examination Examination)
        {
            /*var builder = Builders<Examination>.Filter;
            var filter = builder.Eq("Category", Examination.DoctorId) & builder.Eq("Surname", Doctor.Surname);
            var query = await _dbContext.Doctors
                                .Find(filter)
                                .ToListAsync();*/
            try
            {
                await _dbContext.Examinations.InsertOneAsync(Examination);

            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> UpdateExamination(ObjectId id, Examination Examination)
        {
            try
            {
                ReplaceOneResult actionResult = await _dbContext.Examinations
                                                .ReplaceOneAsync(n => n.Id.Equals(id)
                                                                , Examination
                                                                , new UpdateOptions { IsUpsert = true });
                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> RemoveExamination(string id)
        {
            try
            {
                //Delete the Business
                DeleteResult actionResult = await _dbContext.Examinations.DeleteOneAsync(
                     Builders<Examination>.Filter.Eq("_id", ObjectId.Parse(id)));

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> RemoveAllExaminations()
        {
            try
            {
                DeleteResult actionResult = await _dbContext.Examinations.DeleteManyAsync(new BsonDocument());

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        #endregion


        #region LabRequestTasks
        public async Task<IEnumerable<LabRequest>> GetAllLabRequests(string type)
        {
            var filter = Builders<LabRequest>.Filter.Eq("RequestType", type);
            try
            {
                return await _dbContext.LabRequests.Find(filter).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }

        }

        public async Task<LabRequest> GetLabRequest(string id)
        {
            var filter = Builders<LabRequest>.Filter.Eq("Id", id);

            try
            {
                return await _dbContext.LabRequests
                                .Find(filter)
                                .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task AddLabRequest(LabViewModel campaignViewModel)
        {
            var campaign = campaignViewModel.LabRequest;
            campaign.TimeCreated = DateTime.Now;
            await _dbContext.LabRequests.InsertOneAsync(campaign);
        }

        public async Task<bool> UpdateLabRequest(ObjectId id, LabRequest campaign)
        {
            try
            {
                ReplaceOneResult actionResult = await _dbContext.LabRequests
                                                .ReplaceOneAsync(n => n.Id.Equals(id)
                                                                , campaign
                                                                , new UpdateOptions { IsUpsert = true });
                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> RemoveLabRequest(string id)
        {
            try
            {
                DeleteResult actionResult = await _dbContext.LabRequests.DeleteOneAsync(
                     Builders<LabRequest>.Filter.Eq("_id", ObjectId.Parse(id)));

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }
        #endregion

        #region CampaignCodesTasks
        public async Task<IEnumerable<CampaignCode>> GetAllCampaignCodes()
        {
            try
            {
                return await _dbContext.CampaignCodes.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }

        }

        public async Task AddCampaignCode(CodeViewModel codeViewModel)
        {
            //var builder = Builders<LabRequest>.Filter;
            //var filter = builder.Eq("_id", ObjectId.Parse(codeViewModel.CampaignCode.CampaignId));
            //var query = _dbContext.LabRequests.Find(filter).FirstOrDefault();

            //if (query != null)
            //{
            //    try
            //    {
            //        var update = Builders<LabRequest>.Update.Set("CampaignCodeQty", query.CampaignCodeQty + codeViewModel.Campaign.CampaignCodeQty).CurrentDate("TimeUpdated");
            //        await _dbContext.LabRequests.UpdateOneAsync(filter, update);

            //        int codeQty = codeViewModel.Campaign.CampaignCodeQty;
            //        codeViewModel.Campaign.TimeCreated = DateTime.Now;
            //        codeViewModel.Campaign.CampaignStatus = true;

            //        for (int i = 0; i < codeQty; i++)
            //        {
            //            var codes = new CampaignCode();
            //            {
            //                codes.CampaignId = codeViewModel.CampaignCode.CampaignId;
            //                codes.CodeStatus = true;
            //                codes.TimeCreated = DateTime.Now;
            //                codes.Code = Guid.NewGuid().ToString().ToUpper().Substring(0, 11).Replace("-", string.Empty);
            //            };

            //            await _dbContext.CampaignCodes.InsertOneAsync(codes);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        // log or manage the exception
            //        throw ex;
            //    }

            //}
        }

        public async Task<bool> RemoveCampaignCode(string codeId)
        {
            try
            {
                DeleteResult actionResult = await _dbContext.CampaignCodes.DeleteOneAsync(Builders<CampaignCode>.Filter.Eq("_id", ObjectId.Parse(codeId)));

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        #endregion

        #region PassItOnTasks
        public async Task<LabRequest> PassItOn(string passitCode, string mobileNo)
        {
            var builder = Builders<CampaignCode>.Filter;
            var filter = builder.Eq("Code", passitCode) & builder.Eq("CodeStatus", true);
            var query = await _dbContext.CampaignCodes.Find(filter).FirstOrDefaultAsync();

            if (query != null)
            {
                //Delete Any outstanding PhoneNo blocks
                await _dbContext.CodeFailure.DeleteManyAsync(Builders<CodeFailure>.Filter.And(Builders<CodeFailure>.Filter.Eq("MobileNo", mobileNo), Builders<CodeFailure>.Filter.Eq("NumberStatus", true)));

                var builder2 = Builders<LabRequest>.Filter;
                var filter2 = builder2.Eq("_id", ObjectId.Parse(query.CampaignId)) & builder2.Gt("UsageLimit", query.Usage) & builder2.Eq("CampaignStatus", true);
                return await _dbContext.LabRequests.Find(filter2).FirstOrDefaultAsync();
            }

            return null;

            /*var lookup = await _dbContext.Campaigns.Aggregate().Lookup("CampaignCode", "_id", "CampaignId", "CampaignResult");
            var filter = Builders<BsonDocument>.Filter.Eq("CampaignResult", new BsonDocument { { "Code", passitCode } });
            var result = lookup.Find(filter).ToList();
            */
        }

        public async Task<string> CodeFailure(string mobileNo)
        {
            //Check If Number already is entered for invalid code entry
            var builder = Builders<CodeFailure>.Filter;
            var filter = builder.Eq("MobileNo", mobileNo) & builder.Eq("NumberStatus", true) & builder.Lt("FailureCount", 3);
            var query = await _dbContext.CodeFailure.Find(filter).FirstOrDefaultAsync();

            if (query == null)
            {
                //If no entry, Do first entry
                var codeFailure = new CodeFailure()
                {
                    MobileNo = mobileNo,
                    FailureCount = 1,
                    NumberStatus = true,
                    TimeCreated = DateTime.Now,
                    ReactivationTime = DateTime.Now
                };

                await _dbContext.CodeFailure.InsertOneAsync(codeFailure);
            }
            else
            {
                //If entry exist, do updates to entry
                if (query.FailureCount + 1 == 3)
                {
                    var update = Builders<CodeFailure>.Update.Set("FailureCount", query.FailureCount + 1).Set("NumberStatus", false).Set("ReactivationTime", DateTime.Now.AddHours(24)).Set("SuspendedFor", 24).CurrentDate("TimeUpdated");

                    UpdateResult actionResult = await _dbContext.CodeFailure.UpdateOneAsync(filter, update);

                    if (actionResult.IsAcknowledged && actionResult.ModifiedCount > 0)
                    {
                        return "You phone number has been blocked for 24 hours due to invalid code entries";
                    }
                }
                else
                {
                    var update = Builders<CodeFailure>.Update.Set("FailureCount", query.FailureCount + 1).Set("NumberStatus", true).CurrentDate("TimeUpdated");

                    await _dbContext.CodeFailure.UpdateOneAsync(filter, update);

                    return "Invalid Code";
                }
                
            }

            return "Invalid Code";
        }

        public async Task<bool> NumberCheck(string mobileNo)
        {
            //Check If Number already is entered for invalid code entry
            var builder = Builders<CodeFailure>.Filter;
            var filter = builder.Eq("MobileNo", mobileNo) & builder.Eq("NumberStatus", false) & builder.Gte("FailureCount", 3) & builder.Gt("ReactivationTime", DateTime.Now);
            var query = await _dbContext.CodeFailure.Find(filter).FirstOrDefaultAsync();

            if (query != null)
            {
                return false;
            }
            //DeleteResult actionResult = await _dbContext.CodeFailure.DeleteOneAsync(Builders<CodeFailure>.Filter.Eq("MobileNo", mobileNo));
            return true;
        }

        public async Task<PassItOn> PassItOnCheck(string passitCode ,string mobileNo, string campaignId)
        {
            var builder = Builders<PassItOn>.Filter;
            var filter = builder.Eq("MobileNo", mobileNo) & builder.Eq("CampaignId", campaignId) & builder.Eq("PassitCode", passitCode);
            return await _dbContext.PassItOns.Find(filter).FirstOrDefaultAsync();
        }

        public async Task AddPassItOn(string passitCode, string mobileNo, string campaignId, string prize, string network)
        {
            PassItOn passItOn = new PassItOn();
            passItOn.PassitCode = passitCode;
            passItOn.MobileNo = mobileNo;
            passItOn.CampaignId = campaignId;
            passItOn.RecipientName = "PIO Winner";
            passItOn.Reference = mobileNo;
            passItOn.Amount = prize;
            passItOn.Message = "PassItOn Reward";
            passItOn.Status = false;
            passItOn.TimeCreated = DateTime.Now;
            passItOn.TimeUpdated = DateTime.Now;
            if (network == "024" || network == "054" || network == "055")
            {
                passItOn.ServiceCode = "mtn-money";
            }

            if (network == "020" || network == "050")
            {
                passItOn.ServiceCode = "vodafone-cash";
            }

            if (network == "027" || network == "057")
            {
                passItOn.ServiceCode = "tigo-cash";
            }

            if (network == "026")
            {
                passItOn.ServiceCode = "airtel-money";
            }

            await _dbContext.PassItOns.InsertOneAsync(passItOn);
        }

        public async Task<bool> UpdateCampaignCode(string passitCode, int usageLimit)
        {
            try
            {
                bool codeStatus = true;
                var filter = Builders<CampaignCode>.Filter.Eq("Code", passitCode);
                var query = await _dbContext.CampaignCodes.Find(filter).FirstOrDefaultAsync();

                if (usageLimit <= query.Usage + 1)
                    codeStatus = false;

                var update = Builders<CampaignCode>.Update.Set("Usage", query.Usage + 1).Set("CodeStatus", codeStatus).CurrentDate("TimeUpdated");

                UpdateResult actionResult = await _dbContext.CampaignCodes.UpdateOneAsync(filter, update);

                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<object> GetPassitonAd(string passitCode)
        {
            var builder = Builders<CampaignCode>.Filter;
            var filter = builder.Eq("Code", passitCode) & builder.Eq("CodeStatus", true);
            var query = await _dbContext.CampaignCodes.Find(filter).FirstOrDefaultAsync();

            if (query != null)
            {
                var adbuilder = Builders<AdInfo>.Filter;
                var adfilter = adbuilder.Eq("CampaignId", query.CampaignId) & adbuilder.Eq("AdStatus", true);
                var passAd = _dbContext.AdInfos.Find(adfilter).ToList();

                if (passAd != null)
                {
                    var pAd = new PassAd();
                    foreach (var adInfo in passAd)
                    {
                        if (adInfo.AdType.Equals("PhoneNo-Image"))
                        {
                            pAd.AdType = adInfo.AdType;
                            pAd.ImageUrl = adInfo.AdMedia.GetElement("ImageUrl").Value.ToString();
                        }
                        else if (adInfo.AdType.Equals("PhoneNo-Video"))
                        {
                            pAd.AdType = adInfo.AdType;
                            pAd.VideoUrl = adInfo.AdMedia.GetElement("VideoUrl").Value.ToString();
                            pAd.VideoHost = adInfo.AdMedia.GetElement("VideoHost").Value.ToString();
                        }
                    }
                    //var js = new JavaScriptSerializer();
                    //return js.Serialize(pAd);
                    return pAd;
                }
            }
            return null;
        }

        public async Task<IEnumerable<PassItOn>> GetAllPassItons()
        {
            try
            {
                return await _dbContext.PassItOns.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }

        }
        #endregion

        #region GeneralAdTasks
        public async Task<IEnumerable<Ad>> GetAllGenAds()
        {
            try
            {
                var builder = Builders<AdInfo>.Filter;
                var filter = builder.Type("CampaignId", BsonType.Null); //& builder.Eq("AdStatus", true);
                var genAds = await _dbContext.AdInfos.Find(filter).ToListAsync();

                List<Ad> adInfos = new List<Ad>();

                string adCountry = "";               
                foreach (var ad in genAds)
                {
                    var adInfo = new Ad();

                    adInfo.Id = ad.Id;
                    adInfo.AdType = ad.AdType;
                    
                    for (int i = 0; i < ad.AdCountry.Count; i++)
                    {
                        if (i == 0)
                        {
                            adCountry = ad.AdCountry[i];
                        }
                        else
                        {
                            adCountry += "," + ad.AdCountry[i];
                        }
                    }
                    adInfo.AdCountry = adCountry;
                    adInfo.AdStatus = ad.AdStatus;
                    adInfo.CreatedBy = ad.CreatedBy;
                    adInfo.TimeCreated = ad.TimeCreated;
                    adInfo.UpdatedBy = ad.UpdatedBy;
                    adInfo.TimeUpdated = ad.TimeUpdated;

                    adInfos.Add(adInfo);
                }
                
                return adInfos;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }

        }

        public async Task<AdInfo> GetGenAds(string id)
        {
            var filter = Builders<AdInfo>.Filter.Eq("_id", ObjectId.Parse(id));

            try
            {
                return await _dbContext.AdInfos
                                .Find(filter)
                                .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> UpdateGenAd(AdInfo adInfo)
        {
            try
            {
                ReplaceOneResult actionResult = await _dbContext.AdInfos
                                                .ReplaceOneAsync(n => n.Id.Equals(adInfo.Id)
                                                                , adInfo
                                                                , new UpdateOptions { IsUpsert = true });
                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> RemoveGenAd(string id)
        {
            try
            {
                DeleteResult actionResult = await _dbContext.AdInfos.DeleteOneAsync(
                     Builders<AdInfo>.Filter.Eq("_id", ObjectId.Parse(id)));

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }
        #endregion

        #region CampaignAdTasks
        public async Task<IEnumerable<Ad>> GetAllCampAds()
        {
            try
            {
                //return await _dbContext.AdInfos.Find(Builders<AdInfo>.Filter.Ne("CampaignId", BsonNull.Value)).ToListAsync();

                var builder = Builders<AdInfo>.Filter;
                var filter = builder.Type("CampaignId", BsonType.String); //& builder.Eq("AdStatus", true);
                var campAds = await _dbContext.AdInfos.Find(filter).ToListAsync();

                List<Ad> adInfos = new List<Ad>();

                string adCountry = "";
                foreach (var ad in campAds)
                {
                    var adInfo = new Ad();

                    adInfo.Id = ad.Id;
                    adInfo.CampaignId = ad.CampaignId;
                    adInfo.AdType = ad.AdType;

                    for (int i = 0; i < ad.AdCountry.Count; i++)
                    {
                        if (i == 0)
                        {
                            adCountry = ad.AdCountry[i];
                        }
                        else
                        {
                            adCountry += "," + ad.AdCountry[i];
                        }
                    }
                    adInfo.AdCountry = adCountry;
                    adInfo.AdStatus = ad.AdStatus;
                    adInfo.CreatedBy = ad.CreatedBy;
                    adInfo.TimeCreated = ad.TimeCreated;
                    adInfo.UpdatedBy = ad.UpdatedBy;
                    adInfo.TimeUpdated = ad.TimeUpdated;

                    adInfos.Add(adInfo);
                }

                return adInfos;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }

        }

        public async Task<AdInfo> GetCampAd(string id)
        {
            var filter = Builders<AdInfo>.Filter.Eq("CampaignId", id);

            try
            {
                return await _dbContext.AdInfos
                                .Find(filter)
                                .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> UpdateCampAd(AdInfo adInfo)
        {
            try
            {
                ReplaceOneResult actionResult = await _dbContext.AdInfos
                                                .ReplaceOneAsync(n => n.CampaignId.Equals(adInfo.CampaignId)
                                                                , adInfo
                                                                , new UpdateOptions { IsUpsert = true });
                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> RemoveCampAd(string id)
        {
            try
            {
                DeleteResult actionResult = await _dbContext.AdInfos.DeleteOneAsync(
                     Builders<AdInfo>.Filter.Eq("_id", ObjectId.Parse(id)));

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }
        #endregion

        #region GenerateCardTasks
        public async Task<IEnumerable<CodeCard>> GetAllGenCards()
        {
            try
            {
                return await _dbContext.CodeCard.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }

        }

        public async Task<CodeCard> GetGenCard(string id)
        {
            var filter = Builders<CodeCard>.Filter.Eq("Id", id);

            try
            {
                return await _dbContext.CodeCard.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> RemoveCard(string id)
        {
            try
            {
                DeleteResult actionResult = await _dbContext.CodeCard.DeleteOneAsync(
                     Builders<CodeCard>.Filter.Eq("_id", ObjectId.Parse(id)));

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }
        #endregion

        #region CodeFailureTasks
        public async Task<IEnumerable<CodeFailure>> GetCodeFailures()
        {
            try
            {
                return await _dbContext.CodeFailure.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }

        }

        public async Task<CodeFailure> GetCodeFailure(string id)
        {
            var filter = Builders<CodeFailure>.Filter.Eq("Id", id);

            try
            {
                return await _dbContext.CodeFailure.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> RemoveCodeFailure(string id)
        {
            try
            {
                DeleteResult actionResult = await _dbContext.CodeFailure.DeleteOneAsync(
                     Builders<CodeFailure>.Filter.Eq("_id", ObjectId.Parse(id)));

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }
        #endregion
    }
}