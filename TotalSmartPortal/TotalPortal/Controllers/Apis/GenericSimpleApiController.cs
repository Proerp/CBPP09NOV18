using System;
using System.Net;
using System.Net.Http;
using System.Linq;
using System.Web;
using System.Web.Http;

using AutoMapper;

using TotalBase.Enums;
using TotalModel;
using TotalDTO;
using TotalCore.Services;

using TotalPortal.Builders;
using TotalPortal.ViewModels.Helpers;
using TotalDTO.Commons;
using TotalPortal.APIs.Sessions;


namespace TotalPortal.Controllers.Apis
{
    [Authorize]
    [GenericSimpleApiAuthorizeAttribute]
    public class GenericSimpleApiController<TEntity, TDto, TPrimitiveDto, TSimpleViewModel> : BaseApiController

        where TEntity : class, IPrimitiveEntity, IBaseEntity, new()
        where TDto : class, TPrimitiveDto
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TSimpleViewModel : TDto, ISimpleViewModel, new() //Note: constraints [TSimpleViewModel : TDto] and also [TViewDetailViewModel : TDto  -> in GenericViewDetailController]: is required for this.genericService.Editable(TDto) only!!! If there is any reason need to remove this constraints, just consider for this.genericService.Editable(TDto) only [should change this.genericService.Editable(TDto) only if needed -- means after remove this constraints]
    {
        protected readonly IGenericService<TEntity, TDto, TPrimitiveDto> GenericService;
        private readonly IViewModelSelectListBuilder<TSimpleViewModel> viewModelSelectListBuilder;

        private bool isSimpleCreate;
        private bool isCreateWizard;




        public GenericSimpleApiController(IGenericService<TEntity, TDto, TPrimitiveDto> genericService, IViewModelSelectListBuilder<TSimpleViewModel> viewModelSelectListBuilder)
            : this(genericService, viewModelSelectListBuilder, false, true)
        {
        }

        public GenericSimpleApiController(IGenericService<TEntity, TDto, TPrimitiveDto> genericService, IViewModelSelectListBuilder<TSimpleViewModel> viewModelSelectListBuilder, bool isCreateWizard)
            : this(genericService, viewModelSelectListBuilder, isCreateWizard, false)
        {
        }

        public GenericSimpleApiController(IGenericService<TEntity, TDto, TPrimitiveDto> genericService, IViewModelSelectListBuilder<TSimpleViewModel> viewModelSelectListBuilder, bool isCreateWizard, bool isSimpleCreate)
            : base(genericService)
        {
            this.GenericService = genericService;
            this.viewModelSelectListBuilder = viewModelSelectListBuilder;

            this.isCreateWizard = isCreateWizard;
            this.isSimpleCreate = isSimpleCreate;
        }



        [HttpGet]
        [Route("Index/{id}")]
        [AccessLevelApiAuthorize(GlobalEnums.AccessLevel.Readable)]
        [OnResultExecutingApiFilterAttribute]
        public virtual IHttpActionResult Index(int? id)
        {
            //MVC: ViewBag.SelectedEntityID = id == null ? -1 : (int)id;
            //MVC: ViewBag.ShowDiscount = this.GenericService.GetShowDiscount();

            TSimpleViewModel simpleViewModel = new TSimpleViewModel();

            return Ok(this.InitViewModel(simpleViewModel));
        }






        [HttpGet]
        [Route("Open/{id}")]
        [AccessLevelApiAuthorize(GlobalEnums.AccessLevel.Readable)]
        [OnResultExecutingApiFilterAttribute]
        public virtual IHttpActionResult Open(int? id)
        {
            TSimpleViewModel simpleViewModel = this.GetViewModel(id, GlobalEnums.AccessLevel.Readable, false, false, true);
            if (simpleViewModel == null) return NotFound();

            return Ok(simpleViewModel);
        }




        /// <summary>
        /// Create NEW from an empty ViewModel object
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Create")]
        [AccessLevelApiAuthorize]
        [OnResultExecutingApiFilterAttribute]
        public virtual IHttpActionResult Create()
        {
            if (!this.isSimpleCreate) return BadRequest();


            return Ok(this.TailorViewModel(this.InitViewModelByPrior(this.InitViewModelByDefault(new TSimpleViewModel())))); //Need to call new TSimpleViewModel() to ensure construct TSimpleViewModel object using Constructor!
        }

        [HttpPost]
        [Route("Create")]
        [OnResultExecutingApiFilterAttribute]
        public virtual IHttpActionResult Create(TSimpleViewModel simpleViewModel)
        {
            if (!this.isSimpleCreate) return BadRequest();

            if ((simpleViewModel.SubmitTypeOption == GlobalEnums.SubmitTypeOption.Save || simpleViewModel.SubmitTypeOption == GlobalEnums.SubmitTypeOption.Closed || simpleViewModel.SubmitTypeOption == GlobalEnums.SubmitTypeOption.Create) && this.Save(simpleViewModel))
                return RedirectAfterSave(simpleViewModel);
            else
            {
                return Ok(this.TailorViewModel(simpleViewModel));
            }
        }







        /// <summary>
        /// Create NEW by show a CreateWizard dialog, where user HAVE TO SELECT A RELATIVE OBJECT to INITIALIZE ViewModel, then SUBMIT the ViewModel
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("CreateWizard")]
        [AccessLevelApiAuthorize]
        [OnResultExecutingApiFilterAttribute]
        public virtual IHttpActionResult CreateWizard()
        {
            if (!this.isCreateWizard) return BadRequest();

            return Ok(this.TailorViewModel(this.InitViewModelByDefault(new TSimpleViewModel())));
        }

        /// <summary>
        /// The SUBMITTED ViewModel will be pass to EDIT VIEW to SHOW for editing data
        /// </summary>
        /// <param name="simpleViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateWizard")]
        [OnResultExecutingApiFilterAttribute]
        public virtual IHttpActionResult CreateWizard(TSimpleViewModel simpleViewModel)
        {
            if (!this.isCreateWizard) return BadRequest();

            ModelState.Clear(); //Add this on 10-Dec-2016: When using Required attribute for a Nullable<System.DateTime>, the Kendo().DateTimePickerFor can not pass when submit. Don't know why!!!
            //This ModelState.Clear(): may be a very good idea -may be very very good :), because: we don't need to pre check model error here (Note ... Note: right here then, we always to return Edit view to input data, the: the model will be check by edit view)

            return Ok(this.TailorViewModel(this.DecorateViewModel(this.InitViewModelByPrior(simpleViewModel)))); //MVC===> TO OPEN BY VIEW: "Edit"
        }






        [HttpGet]
        [Route("Edit/{id}")]
        [AccessLevelApiAuthorize(GlobalEnums.AccessLevel.Readable)]
        [OnResultExecutingApiFilterAttribute]
        public virtual IHttpActionResult Edit(int? id)
        {
            TSimpleViewModel simpleViewModel = this.GetViewModel(id, GlobalEnums.AccessLevel.Readable);
            if (simpleViewModel == null) return BadRequest();

            return Ok(simpleViewModel);
        }


        /// <summary>
        /// Use SubmitTypeOption to DISTINGUISH two type of submit:
        ///     1.SubmitTypeOption.Save: Submit by EDIT VIEW to SAVE ViewModel
        ///     2.SubmitTypeOption.Popup: Submit by CreateWizard dialog, where user BE ABLE TO CHANGE A RELATIVE OBJECT to current ViewModel, then SUBMIT the ViewModel (Note on: 07/07/2015: for example: User may want to change the current edited purchase invoice to adapt to another purchase order - THE RELATIVE OBJECT)
        /// </summary>
        /// <param name="simpleViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Edit")]
        [OnResultExecutingApiFilterAttribute]
        public virtual IHttpActionResult Edit(TSimpleViewModel simpleViewModel)
        {
            if ((simpleViewModel.SubmitTypeOption == GlobalEnums.SubmitTypeOption.Save || simpleViewModel.SubmitTypeOption == GlobalEnums.SubmitTypeOption.Closed || simpleViewModel.SubmitTypeOption == GlobalEnums.SubmitTypeOption.Create) && this.Save(simpleViewModel))
                return RedirectAfterSave(simpleViewModel);
            else
            {
                if (simpleViewModel.SubmitTypeOption == GlobalEnums.SubmitTypeOption.Popup) this.DecorateViewModel(simpleViewModel);
                return Ok(this.TailorViewModel(simpleViewModel));
            }
        }





        public virtual IHttpActionResult RedirectAfterSave(TSimpleViewModel simpleViewModel)
        {
            return Ok(simpleViewModel); //WEB API: RETURN OK FOR ALL USE CASE





            if (simpleViewModel.SubmitTypeOption == GlobalEnums.SubmitTypeOption.Create)
                if (this.isSimpleCreate)
                    return SBTRedirect("Create");
                else
                {
                    TSimpleViewModel createWizardViewModel = InitViewModelByCopy(simpleViewModel);
                    if (createWizardViewModel == null) return BadRequest();
                    return CreateWizard(createWizardViewModel);
                }
            else
            {
                //MVC: if (simpleViewModel.PrintAfterClosedSubmit) this.TempData["PrintOptionID"] = simpleViewModel.PrintOptionID;
                return SBTRedirect(simpleViewModel.SubmitTypeOption == GlobalEnums.SubmitTypeOption.Save ? "Edit" : simpleViewModel.PrintAfterClosedSubmit ? "Print" : "Index", new { id = simpleViewModel.GetID() });
            }
        }

        private IHttpActionResult SBTRedirect(string action) { return this.SBTRedirect(action, null); }
        private IHttpActionResult SBTRedirect(string action, object id)
        {
            var response = Request.CreateResponse(HttpStatusCode.Moved);
            string fullyQualifiedUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);
            response.Headers.Location = new Uri(fullyQualifiedUrl);

            return Ok(response);
        }

        #region Approve/ UnApprove

        [HttpGet]
        [Route("Approve/{id}")]
        [AccessLevelApiAuthorize(GlobalEnums.AccessLevel.Readable)]
        [OnResultExecutingApiFilterAttribute]
        public virtual IHttpActionResult Approve(int? id)
        {
            TSimpleViewModel simpleViewModel = this.GetViewModel(id, GlobalEnums.AccessLevel.Readable, true);
            if (simpleViewModel == null) return BadRequest();

            if (!simpleViewModel.Approved)
                if (this.GenericService.GetApprovalPermitted(simpleViewModel.OrganizationalUnitID))
                    simpleViewModel.Approvable = this.GenericService.Approvable(simpleViewModel);
                else //USER DON'T HAVE PERMISSION TO DO
                    return Unauthorized();

            if (simpleViewModel.Approved)
                if (this.GenericService.GetUnApprovalPermitted(simpleViewModel.OrganizationalUnitID))
                    simpleViewModel.UnApprovable = this.GenericService.UnApprovable(simpleViewModel);
                else //USER DON'T HAVE PERMISSION TO DO
                    return Unauthorized();

            return Ok(simpleViewModel);
        }

        [HttpPost]
        [Route("Approve")]
        public virtual IHttpActionResult ApproveConfirmed(TSimpleViewModel simpleViewModel)
        {
            try
            {
                if (this.GenericService.ToggleApproved(simpleViewModel))
                    return this.SBTRedirect("Index");
                else
                    throw new System.ArgumentException("Lỗi vô hiệu dữ liệu", "Dữ liệu này không thể vô hiệu.");
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception);
                return SBTRedirect("Approve", simpleViewModel.GetID());
            }
        }


        #endregion Approve/ UnApprove




        [HttpGet]
        [Route("Delete/{id}")]
        [AccessLevelApiAuthorize]
        [OnResultExecutingApiFilterAttribute]
        public virtual IHttpActionResult Delete(int? id)
        {
            TSimpleViewModel simpleViewModel = this.GetViewModel(id, GlobalEnums.AccessLevel.Editable, true);
            if (simpleViewModel == null) return BadRequest();

            return Ok(simpleViewModel);
        }


        [HttpPost]
        [Route("Delete")]
        public virtual IHttpActionResult DeleteConfirmed(int id)
        {
            try
            {
                if (this.GenericService.Delete(id))
                    return this.SBTRedirect("Index");
                else
                    throw new System.ArgumentException("Lỗi xóa dữ liệu", "Dữ liệu này không thể xóa được.");

            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception);
                return this.SBTRedirect("Delete", id);
            }
        }






        [HttpGet]
        [Route("Alter/{id}")]
        [AccessLevelApiAuthorize]
        [OnResultExecutingApiFilterAttribute]
        public virtual IHttpActionResult Alter(int? id)
        {
            TSimpleViewModel simpleViewModel = this.GetViewModel(id, GlobalEnums.AccessLevel.Editable, false, true);
            if (simpleViewModel == null) return BadRequest();

            return Ok(simpleViewModel);
        }


        [HttpPost]
        [Route("Alter")]
        public virtual IHttpActionResult AlterConfirmed(TSimpleViewModel simpleViewModel)
        {
            try
            {
                if (this.GenericService.Alter(simpleViewModel))
                    return this.SBTRedirect("Index");
                else
                    throw new System.ArgumentException("Lỗi vô hiệu dữ liệu", "Dữ liệu này không thể vô hiệu.");

            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception);
                return Ok(this.TailorViewModel(simpleViewModel, false, true)); //MVC: SHOW VIEW "Alter" AGAIN 
                //return this.SBTRedirect("Alter", simpleViewModel.GetID());
            }
        }











        #region Void/ UnVoid

        [HttpGet]
        [Route("Void/{id}")]
        [AccessLevelApiAuthorize(GlobalEnums.AccessLevel.Readable)]
        [OnResultExecutingApiFilterAttribute]
        public virtual IHttpActionResult Void(int? id)
        {
            TSimpleViewModel simpleViewModel = this.GetViewModel(id, GlobalEnums.AccessLevel.Readable, true);
            if (simpleViewModel == null) return BadRequest();

            if (!simpleViewModel.InActive)
                if (this.GenericService.GetVoidablePermitted(simpleViewModel.OrganizationalUnitID))
                {
                    simpleViewModel.Voidable = this.GenericService.Voidable(simpleViewModel);
                    //MVC: RequireJsOptions.Add("Voidable", simpleViewModel.Voidable, RequireJsOptionsScope.Page);
                }
                else //USER DON'T HAVE PERMISSION TO DO
                    return Unauthorized();

            if (simpleViewModel.InActive)
                if (this.GenericService.GetUnVoidablePermitted(simpleViewModel.OrganizationalUnitID))
                    simpleViewModel.UnVoidable = this.GenericService.UnVoidable(simpleViewModel);
                else //USER DON'T HAVE PERMISSION TO DO
                    return Unauthorized();

            return Ok(simpleViewModel);
        }

        [HttpPost]
        [Route("Void")]
        public virtual IHttpActionResult VoidConfirmed(TSimpleViewModel simpleViewModel)
        {
            try
            {
                if (simpleViewModel.VoidTypeID == null || simpleViewModel.VoidTypeID <= 0) throw new System.ArgumentException("Lỗi hủy dữ liệu", "Vui lòng nhập lý do hủy đơn hàng.");

                if (this.GenericService.ToggleVoid(simpleViewModel))
                    return this.SBTRedirect("Index");
                else
                    throw new System.ArgumentException("Lỗi duyệt dữ liệu", "Dữ liệu này không thể duyệt được.");
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception);
                return this.SBTRedirect("Void", simpleViewModel.GetID());
            }
        }


        #endregion Void/ UnVoid


        #region VoidDetail/ UnVoidDetail

        [HttpGet]
        [Route("VoidDetail/{id}/{detailID}")]
        [AccessLevelApiAuthorize(GlobalEnums.AccessLevel.Readable)]
        [OnResultExecutingApiFilterAttribute]
        public virtual IHttpActionResult VoidDetail(int? id, int? detailID)
        {
            TSimpleViewModel simpleViewModel = this.GetViewModel(id, GlobalEnums.AccessLevel.Readable, true);
            if (simpleViewModel == null) return BadRequest();

            return Ok(this.PrepareVoidDetail(simpleViewModel, detailID));
        }

        [HttpPost]
        [Route("VoidDetail")]
        public virtual IHttpActionResult VoidDetailConfirmed(TotalPortal.ViewModels.Helpers.VoidDetailViewModel voidDetailViewModel)
        {
            try
            {
                if (voidDetailViewModel.VoidTypeID == null || voidDetailViewModel.VoidTypeID <= 0) throw new System.ArgumentException("Lỗi hủy dữ liệu", "Vui lòng nhập lý do hủy đơn hàng.");

                TEntity entity = this.GetEntityAndCheckAccessLevel(voidDetailViewModel.ID, GlobalEnums.AccessLevel.Readable);
                if (entity == null) throw new System.ArgumentException("Lỗi hủy dữ liệu", "BadRequest.");

                TDto dto = Mapper.Map<TDto>(entity);

                if (this.GenericService.ToggleVoidDetail(dto, voidDetailViewModel.DetailID, voidDetailViewModel.InActivePartial, (int)voidDetailViewModel.VoidTypeID))
                {
                    ModelState.Clear(); ////https://weblog.west-wind.com/posts/2012/apr/20/aspnet-mvc-postbacks-and-htmlhelper-controls-ignoring-model-changes
                    //IMPORTANT NOTES: ASP.NET MVC Postbacks and HtmlHelper Controls ignoring Model Changes
                    //HtmlHelpers controls (like .TextBoxFor() etc.) don't bind to model values on Postback, but rather get their value directly out of the POST buffer from ModelState. Effectively it looks like you can't change the display value of a control via model value updates on a Postback operation. 
                    //When MVC binds controls like @Html.TextBoxFor() or @Html.TextBox(), it always binds values on a GET operation. On a POST operation however, it'll always used the AttemptedValue to display the control. MVC binds using the ModelState on a POST operation, not the model's value
                    //So, if you want the behavior that I was expecting originally you can actually get it by clearing the ModelState in the controller code: ModelState.Clear();
                    voidDetailViewModel.InActivePartial = !voidDetailViewModel.InActivePartial;
                    return Ok(voidDetailViewModel); //MVC: "VoidDetailSuccess", 
                }
                else
                    throw new System.ArgumentException("Lỗi hủy dữ liệu", "Dữ liệu này không thể hủy được.");
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception);
                return this.SBTRedirect("VoidDetail", new { @id = voidDetailViewModel.ID, @detailId = voidDetailViewModel.DetailID });
            }
        }

        private TSimpleViewModel PrepareVoidDetail(TSimpleViewModel simpleViewModel, int? detailId)
        {
            simpleViewModel.PrepareVoidDetail(detailId);
            return simpleViewModel;
        }


        #endregion VoidDetail/ UnVoidDetail









        protected virtual TEntity GetEntityAndCheckAccessLevel(int? id, GlobalEnums.AccessLevel accessLevel)
        {
            TEntity entity;
            if (id == null || (entity = this.GenericService.GetByID((int)id)) == null) return null;

            if (this.GenericService.GetAccessLevel(entity.OrganizationalUnitID) < accessLevel) return null;

            return entity;
        }



        protected virtual bool Save(TSimpleViewModel simpleViewModel)
        {
            try
            {
                if (!ModelState.IsValid) return false;//Check Viewmodel IsValid

                TDto dto = simpleViewModel;// Mapper.Map<TSimpleViewModel, TDto>(simpleViewModel);//Convert from Viewmodel to DTO

                //MVC: if (!this.TryValidateModel(dto)) return false;//Check DTO IsValid
                //MVC: else
                if (this.GenericService.Save(dto))
                {
                    simpleViewModel.SetID(dto.GetID());
                    this.BackupViewModelToSession(simpleViewModel);

                    return true;
                }
                else
                    return false;

            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception);
                return false;
            }
        }












        protected virtual TSimpleViewModel MapEntityToViewModel(TEntity entity)
        {
            TSimpleViewModel simpleViewModel = Mapper.Map<TSimpleViewModel>(entity);

            return simpleViewModel;
        }



        protected virtual TSimpleViewModel InitViewModelByPrior(TSimpleViewModel simpleViewModel)
        {
            return simpleViewModel;
        }


        protected virtual TSimpleViewModel InitViewModel(TSimpleViewModel simpleViewModel)
        {
            return simpleViewModel;
        }

        /// <summary>
        /// Init new viewmodel by set default value. Default, this procedure does nothing and just return the passing parameter simpleViewModel.
        /// Each module should override this InitViewModelByDefault to init its's viewmodel accordingly, if needed
        /// </summary>
        /// <param name="simpleViewModel"></param>
        /// <returns></returns>
        protected virtual TSimpleViewModel InitViewModelByDefault(TSimpleViewModel simpleViewModel)
        {
            return this.InitViewModel(simpleViewModel);
        }


        /// <summary>
        /// Backup ViewModel to HttpContext.Session for reuse later
        /// </summary>
        /// <param name="simpleViewModel"></param>
        protected virtual void BackupViewModelToSession(TSimpleViewModel simpleViewModel) { }


        /// <summary>
        /// Init new viewmodel by copy from current viewmodel object. Default, this procedure does nothing and return null.
        /// Each module should override this InitViewModelByCopy to init its's viewmodel accordingly, if needed
        /// </summary>
        /// <param name="simpleViewModel"></param>
        /// <returns></returns>
        protected virtual TSimpleViewModel InitViewModelByCopy(TSimpleViewModel simpleViewModel)
        {
            return null;
        }




        private TSimpleViewModel BuildViewModel(TEntity entity, bool forDelete, bool forAlter, bool forOpen)
        {
            return this.TailorViewModel(this.DecorateViewModel(this.MapEntityToViewModel(entity)), forDelete, forAlter, forOpen);
        }

        protected virtual TSimpleViewModel TailorViewModel(TSimpleViewModel simpleViewModel)
        {
            return this.TailorViewModel(simpleViewModel, false);
        }

        protected virtual TSimpleViewModel TailorViewModel(TSimpleViewModel simpleViewModel, bool forDelete)
        {
            return this.TailorViewModel(simpleViewModel, forDelete, false);
        }

        protected virtual TSimpleViewModel TailorViewModel(TSimpleViewModel simpleViewModel, bool forDelete, bool forAlter)
        {
            return this.TailorViewModel(simpleViewModel, forDelete, forAlter, false);
        }

        protected virtual TSimpleViewModel TailorViewModel(TSimpleViewModel simpleViewModel, bool forDelete, bool forAlter, bool forOpen)
        {
            if (!forOpen)
            {
                if (!forDelete)//Be caution: the value of simpleViewModel.Editable should be SET EVERY TIME THE simpleViewModel LOADED! This means: if it HAVEN'T SET YET, the default value of simpleViewModel.Editable is FALSE               (THE CONDITIONAL CLAUSE: if (!forDelete) MEAN: WHEN SHOW VIEW FOR DELETE, NO NEED TO CHECK Editable => Editable SHOULD BE FALSE)
                    simpleViewModel.Editable = this.GenericService.Editable(simpleViewModel);

                if (forDelete) // || simpleViewModel is ServiceContractViewModel                    //WHEN forDelete, IT SHOULD BE CHECK FOR Deletable ATTRIBUTE, SURELY.          BUT, WHEN OPEN VIEW FOR EDIT, NOW: ONLY VIEW ServiceContract NEED TO USE Deletable ATTRIBUTE ONLY. SO, THIS CODE IS CORRECT FOR NOW, BUT LATER, IF THERE IS MORE VIEWS NEED THIS Deletable ATTRIBUTE, THIS CODE SHOULD MODIFY MORE GENERIC!!!
                    simpleViewModel.Deletable = this.GenericService.Deletable(simpleViewModel);

                if (forAlter)//NOW THIS GlobalLocked attribute ONLY be considered WHEN ALTER ACTION to USE IN ALTER VIEW: to ALLOW or NOT ALTER.
                    simpleViewModel.GlobalLocked = this.GenericService.GlobalLocked(simpleViewModel);
            }

            simpleViewModel.ShowDiscount = this.GetShowDiscount(simpleViewModel);

            simpleViewModel.ShowListedPrice = this.GetShowListedPrice(simpleViewModel);
            simpleViewModel.ShowListedGrossPrice = this.GetShowListedGrossPrice(simpleViewModel);

            //MVC: RequireJsOptions.Add("Editable", simpleViewModel.Editable, RequireJsOptionsScope.Page);
            //MVC: RequireJsOptions.Add("Deletable", simpleViewModel.Deletable, RequireJsOptionsScope.Page);

            simpleViewModel.UserID = this.GenericService.UserID; //CAU LENH NAY TAM THOI DUOC SU DUNG DE SORT USER DROPDWONLIST. SAU NAY NEN LAM CACH KHAC, CACH NAY KHONG HAY
            simpleViewModel.LocationID = this.GenericService.LocationID; //CAU LENH NAY TAM THOI DUOC SU DUNG DE SORT USER DROPDWONLIST. SAU NAY NEN LAM CACH KHAC, CACH NAY KHONG HAY

            this.viewModelSelectListBuilder.BuildSelectLists(simpleViewModel); //Buil select list for dropdown box using IEnumerable<SelectListItem> (using for short data list only). For the long list, it should use Kendo automplete instead.

            return simpleViewModel;
        }

        protected virtual TSimpleViewModel DecorateViewModel(TSimpleViewModel simpleViewModel)
        {
            return simpleViewModel;
        }

        protected virtual bool GetShowDiscount(TSimpleViewModel simpleViewModel)
        {
            return this.GenericService.GetShowDiscount();
        }

        protected virtual bool GetShowListedPrice(TSimpleViewModel simpleViewModel)
        {
            return this.GenericService.GetShowListedPrice(0);
        }

        protected virtual bool GetShowListedGrossPrice(TSimpleViewModel simpleViewModel)
        {
            return this.GenericService.GetShowListedGrossPrice(0);
        }

        #region GetViewModel
        /// <summary>
        /// There are serveral times have to build ViewModel from Entity, by the same steps
        /// This is the reason to have GetViewModel. This is not for any purpose. This GetViewModel may change or ormit if needed
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected TSimpleViewModel GetViewModel(int? id, GlobalEnums.AccessLevel accessLevel)
        {
            return this.GetViewModel(id, accessLevel, false);
        }

        protected TSimpleViewModel GetViewModel(int? id, GlobalEnums.AccessLevel accessLevel, bool forDelete)
        {
            return this.GetViewModel(id, accessLevel, forDelete, false);
        }

        protected TSimpleViewModel GetViewModel(int? id, GlobalEnums.AccessLevel accessLevel, bool forDelete, bool forAlter)
        {
            return this.GetViewModel(id, accessLevel, forDelete, forAlter, false);
        }

        protected TSimpleViewModel GetViewModel(int? id, GlobalEnums.AccessLevel accessLevel, bool forDelete, bool forAlter, bool forOpen)
        {
            TEntity entity = this.GetEntityAndCheckAccessLevel(id, accessLevel);
            if (entity == null) return null;

            return this.BuildViewModel(entity, forDelete, forAlter, forOpen);
        }
        #endregion GetViewModel



        [HttpGet]
        [Route("Print/{id}/{detailID}")]
        [OnResultExecutingApiFilterAttribute]
        public IHttpActionResult Print(int? id, int? detailID)
        {
            return Ok(InitPrintViewModel(id, detailID));
        }

        //protected virtual PrintViewModel InitPrintViewModel(int? id) { return this.InitPrintViewModel(id, null); }
        protected virtual PrintViewModel InitPrintViewModel(int? id, int? detailID)
        {
            PrintViewModel printViewModel = new PrintViewModel() { Id = id != null ? (int)id : 0, DetailID = detailID };
            //MVC: if (this.TempData["PrintOptionID"] != null)
            //MVC:     printViewModel.PrintOptionID = (int)this.TempData["PrintOptionID"];
            return printViewModel;
        }

        //Create/CreateWizard: by Authorize Attribute (Editable)
        //Edit: by Authorize Attribute (Readonly?) -> then: Get Entity by ID (Need to check editable ACCESS for entity) -> Check Ediatable of Entity (by service) -> Add FLAG STATUS for Editable/ Readonly -> Set View Status!
        //Index: by Authorize Attribute (Readonly) -> Then load entity list by permission check
        //Save: Check for Ediable for entity (Should check by servicelayer only?)

    }

}