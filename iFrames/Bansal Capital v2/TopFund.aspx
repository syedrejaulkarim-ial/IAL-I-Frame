

<!doctype html>
<html lang="en">

<head>
    <title>Market Watch</title>
     <!-- Bootstrap Css -->
    <link href="assets/css/bootstrap.min.css" id="bootstrap-style" rel="stylesheet" type="text/css" />
    <!-- Icons Css -->
    <link href="assets/css/icons.min.css" rel="stylesheet" type="text/css" />
    <!-- App Css-->
    <link href="assets/css/app.min.css" id="app-style" rel="stylesheet" type="text/css" />
 
     <!-- nouisliderribute css -->
 <link rel="stylesheet" href="assets/libs/nouislider/nouislider.min.css">
</head>

<body>
     <!-- Begin page -->
    <div id="layout-wrapper">

        <!-- ============================================================== -->
        <!-- Start right Content here -->
        <!-- ============================================================== -->
        <div class="main-content">
            <div class="page-content">
                <div class="container-fluid">
                    <div class="row">
                    </div>

                    <div class="row">
                        <div class="col-xl-12">
                            <div class="card">
                                <div class="card-header">
                                    <div>
                                        <ul class="nav nav-pills nav-justified bg-light m-3 rounded" role="tablist">
                                            <li class="nav-item waves-effect waves-light">
                                                <a class="nav-link active" data-bs-toggle="tab" href="#TopFund"
                                                    role="tab">
                                                    <!-- <span class="d-block d-sm-none"><i class="fas fa-home"></i></span> -->
                                                    <span class="d-block d-sm-block">Top Fund</span>
                                                </a>
                                            </li>
                                            <li class="nav-item waves-effect waves-light">
                                                <a class="nav-link" data-bs-toggle="tab" href="#SearchFunds" role="tab">
                                                    <!-- <span class="d-block d-sm-none"><i class="far fa-user"></i></span> -->
                                                    <span class="d-block d-sm-block">Search Funds</span>
                                                </a>
                                            </li>
                                            <li class="nav-item waves-effect waves-light">
                                                <a class="nav-link" data-bs-toggle="tab" href="#CompareFunds"
                                                    role="tab">
                                                    <!-- <span class="d-block d-sm-none"><i class="far fa-envelope"></i></span> -->
                                                    <span class="d-block d-sm-block">Compare Funds</span>
                                                </a>
                                            </li>
                                            <li class="nav-item waves-effect waves-light">
                                                <a class="nav-link" data-bs-toggle="tab" href="#NavTraker" role="tab">
                                                    <!-- <span class="d-block d-sm-none"><i class="far fa-envelope"></i></span> -->
                                                    <span class="d-block d-sm-block">Nav Tracker</span>
                                                </a>
                                            </li>
                                        </ul>
                                    </div>
                                </div><!-- end card header -->

                                <div class="card-body">
                                    <!-- Tab panes -->
                                    <div class="tab-content text-muted">
                                        <div class="tab-pane active" id="TopFund" role="tabpanel">
                                            <div class="col-xl-12">
                                                <div class="card">
                                                    <div class="card-body" style="padding: 0;">
                                                        <!-- Nav tabs -->
                                                        <ul class="nav nav-tabs nav-tabs-custom nav-justified"
                                                            role="tablist">
                                                            <li class="nav-item">
                                                                <a class="nav-link active" data-bs-toggle="tab"
                                                                    href="#home1" role="tab">
                                                                    <span class="d-block d-sm-none"><i
                                                                            class="fas fa-home"></i></span>
                                                                    <span class="d-none d-sm-block">Small cap
                                                                    </span>
                                                                </a>
                                                            </li>
                                                            <li class="nav-item">
                                                                <a class="nav-link" data-bs-toggle="tab"
                                                                    href="#profile1" role="tab">
                                                                    <span class="d-block d-sm-none"><i
                                                                            class="far fa-user"></i></span>
                                                                    <span class="d-none d-sm-block">Mid cap</span>
                                                                </a>
                                                            </li>
                                                            <li class="nav-item">
                                                                <a class="nav-link" data-bs-toggle="tab"
                                                                    href="#messages1" role="tab">
                                                                    <span class="d-block d-sm-none"><i
                                                                            class="far fa-envelope"></i></span>
                                                                    <span class="d-none d-sm-block">Large cap</span>
                                                                </a>
                                                            </li>
                                                            <li class="nav-item">
                                                                <a class="nav-link" data-bs-toggle="tab"
                                                                    href="#settings1" role="tab">
                                                                    <span class="d-block d-sm-none"><i
                                                                            class="fas fa-cog"></i></span>
                                                                    <span class="d-none d-sm-block">Multicap</span>
                                                                </a>
                                                            </li>
                                                            <li class="nav-item">
                                                                <a class="nav-link" data-bs-toggle="tab"
                                                                    href="#Flexicap1" role="tab">
                                                                    <span class="d-block d-sm-none"><i
                                                                            class="fas fa-cog"></i></span>
                                                                    <span class="d-none d-sm-block">Flexi cap</span>
                                                                </a>
                                                            </li>
                                                            <li class="nav-item">
                                                                <a class="nav-link" data-bs-toggle="tab" href="#Hybrid1"
                                                                    role="tab">
                                                                    <span class="d-block d-sm-none"><i
                                                                            class="fas fa-cog"></i></span>
                                                                    <span class="d-none d-sm-block">Hybrid</span>
                                                                </a>
                                                            </li>
                                                            <li class="nav-item">
                                                                <a class="nav-link" data-bs-toggle="tab" href="#Debt1"
                                                                    role="tab">
                                                                    <span class="d-block d-sm-none"><i
                                                                            class="fas fa-cog"></i></span>
                                                                    <span class="d-none d-sm-block">Debt</span>
                                                                </a>
                                                            </li>
                                                            <li class="nav-item">
                                                                <a class="nav-link" data-bs-toggle="tab"
                                                                    href="#Thematic1" role="tab">
                                                                    <span class="d-block d-sm-none"><i
                                                                            class="fas fa-cog"></i></span>
                                                                    <span class="d-none d-sm-block">Thematic</span>
                                                                </a>
                                                            </li>
                                                            <li class="nav-item">
                                                                <a class="nav-link" data-bs-toggle="tab" href="#ELSS1"
                                                                    role="tab">
                                                                    <span class="d-block d-sm-none"><i
                                                                            class="fas fa-cog"></i></span>
                                                                    <span class="d-none d-sm-block">ELSS</span>
                                                                </a>
                                                            </li>
                                                        </ul>

                                                        <!-- Tab panes -->
                                                        <div class="tab-content p-3 text-muted">
                                                            <div class="tab-pane active" id="home1" role="tabpanel">
                                                                <div class="card">
                                                                    <div class="card-body pb-xl-2">
                                                                        <h4 class="font-size-20 mb-1"><a href=""
                                                                                class="text-dark">Axis Growth
                                                                                Opportunities Fund-Growth
                                                                            </a></h4>
                                                                        <div class="col-xl-12">
                                                                            <div class="row">
                                                                                <div class="col-xl-4">
                                                                                    <div class="col-xl-12">
                                                                                        <p>12.953 <span class="mdi mdi-arrow-up-bold text-success"></span> + .0156 ( +1.19%)
                                                                                        </p>
                                                                                    </div>
                                                                                    <div class="col-xl-12">
                                                                                        <div class="row">
                                                                                            <div class="col">
                                                                                                <div>
                                                                                                    <h6 style="margin-bottom: 0px;">
                                                                                                        13.45 <small class="text-muted">(20 Jan 2023)</small>
                                                                                                    </h6>
                                                                                                    <p class="text-muted mb-0">
                                                                                                        <span class="badge  badge-soft-success font-size-12">High Nav</span>
                                                                                                    </p>
                                                                                                </div>
                                                                                            </div>
                                                                                            <div class="col">
                                                                                                <div>
                                                                                                    <h6 style="margin-bottom: 0px;">
                                                                                                        11.23 <small class="text-muted">(28 Oct 2023)</small>
                                                                                                    </h6>
                                                                                                    <p
                                                                                                        class="text-muted mb-0">
                                                                                                        <span class="badge  badge-soft-danger  font-size-12">Low Nav</span>
                                                                                                    </p>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>

                                                                                <div class="col-xl-8">
                                                                                    <div class="row">
                                                                                        <div class="col border-end">
                                                                                            <div class="text-center">
                                                                                                <p class="text-muted mb-0">6 months</p>
                                                                                                <h6>12.09%</h6>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="col border-end">
                                                                                            <div class="text-center">
                                                                                                <p class="text-muted mb-0">1 year</p>
                                                                                                <h6>14.52%</h6>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="col border-end">
                                                                                            <div class="text-center">
                                                                                                <p class="text-muted mb-0">3 years</p>
                                                                                                <h6>24.82%</h6>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="col border-end">
                                                                                            <div class="text-center">
                                                                                                <p class="text-muted mb-0"> Since inception</p>
                                                                                                <h6>16.78%</h6>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="col">
                                                                                            <div class="text-center">
                                                                                                <p class="text-muted mb-0"> Fund size (Cr)</p>
                                                                                                <h6>2,324.14</h6>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="row">
                                                                                        <div class="col-sm-2 mt-2">
                                                                                            <p class="mt-4 mb-0" style="text-align: right;">18.09% </p>
                                                                                        </div>
                                                                                        <div class="col-xl-8 mt-2">
                                                                                            <div class="row align-items-center g-0">
                                                                                                <div class="col-sm-12 mt-2">
                                                                                                    <p class="text-muted mb-0">
                                                                                                        <b>Average Category Returns(3  yrs)</b>
                                                                                                    </p>
                                                                                                </div>

                                                                                                <div class="col-sm-12">
                                                                                                    <div class="progress progress-md mt-1">
                                                                                                        <div class="progress-bar progress-bar-striped bg-primary"
                                                                                                            role="progressbar"
                                                                                                            style="width: 72%"
                                                                                                            aria-valuenow="52"
                                                                                                            aria-valuemin="0"
                                                                                                            aria-valuemax="52">
                                                                                                        </div>
                                                                                                    </div>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="col-sm-2 mt-2">
                                                                                            <p class="mt-4 mb-0" style="text-align: left;"> 27.56%</p>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="row">
                                                                                <div class="col-sm-12 mt-2" style="text-align: right; padding: 0px;">
                                                                                    <a href="" class="text-primary fw-semibold">
                                                                                        <u>More details</u> 
                                                                                        <i class="mdi mdi-arrow-right ms-1 align-middle"></i></a>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="card">
                                                                    <div class="card-body pb-xl-2">
                                                                        <h4 class="font-size-20 mb-1"><a href="" class="text-dark">Kotak Mutual Multicap Fund-Growth </a></h4>
                                                                        <div class="col-xl-12">
                                                                            <div class="row">
                                                                                <div class="col-xl-4">
                                                                                    <div class="col-xl-12">
                                                                                        <p> 12.953 <span class="mdi mdi-arrow-up-bold text-success"></span> + .0156 ( +1.19%)
                                                                                        </p>
                                                                                    </div>
                                                                                    <div class="col-xl-12">
                                                                                        <div class="row">
                                                                                            <div class="col">
                                                                                                <div>
                                                                                                    <h6 style="margin-bottom: 0px;">13.45 <small class="text-muted">(20 Jan 2023)</small>
                                                                                                    </h6>
                                                                                                    <p class="text-muted mb-0">
                                                                                                        <span class="badge badge-soft-success font-size-12">High Nav</span>
                                                                                                    </p>
                                                                                                </div>
                                                                                            </div>
                                                                                            <div class="col">
                                                                                                <div>
                                                                                                    <h6 style="margin-bottom: 0px;">
                                                                                                        11.23 <small class="text-muted">(28 Oct 2023)</small>
                                                                                                    </h6>
                                                                                                    <p class="text-muted mb-0">
                                                                                                        <span class="badge badge-soft-danger font-size-12">Low Nav</span>
                                                                                                    </p>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>

                                                                                <div class="col-xl-8">
                                                                                    <div class="row">
                                                                                        <div class="col border-end">
                                                                                            <div class="text-center">
                                                                                                <p class="text-muted mb-0"> 6 months</p>
                                                                                                <h6>12.09%</h6>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="col border-end">
                                                                                            <div class="text-center">
                                                                                                <p class="text-muted mb-0">
                                                                                                    1 year</p>
                                                                                                <h6>14.52%</h6>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="col border-end">
                                                                                            <div class="text-center">
                                                                                                <p class="text-muted mb-0">3 years</p>
                                                                                                <h6>24.82%</h6>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="col border-end">
                                                                                            <div class="text-center">
                                                                                                <p class="text-muted mb-0"> Since inception</p>
                                                                                                <h6>16.78%</h6>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="col">
                                                                                            <div class="text-center">
                                                                                                <p class="text-muted mb-0">
                                                                                                    Fund size (Cr)</p>
                                                                                                <h6>2,324.14</h6>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="row">
                                                                                        <div class="col-sm-2 mt-2">
                                                                                            <p class="mt-4 mb-0" style="text-align: right;">18.09% </p>
                                                                                        </div>
                                                                                        <div class="col-xl-8 mt-2">
                                                                                            <div class="row align-items-center g-0">
                                                                                                <div class="col-sm-12 mt-2">
                                                                                                    <p class="text-muted mb-0">
                                                                                                        <b>Average Category Returns(3
                                                                                                            yrs)</b>
                                                                                                    </p>
                                                                                                </div>

                                                                                                <div class="col-sm-12">
                                                                                                    <div class="progress progress-md mt-1">
                                                                                                        <div class="progress-bar progress-bar-striped bg-primary"
                                                                                                            role="progressbar"
                                                                                                            style="width: 72%"
                                                                                                            aria-valuenow="52"
                                                                                                            aria-valuemin="0"
                                                                                                            aria-valuemax="52">
                                                                                                        </div>
                                                                                                    </div>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="col-sm-2 mt-2">
                                                                                            <p class="mt-4 mb-0" style="text-align: left;"> 27.56%</p>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-sm-12 mt-2" style="text-align: right; padding: 0px;">
                                                                            <a href="" class="text-primary fw-semibold">
                                                                                <u>More details</u>
                                                                                <i class="mdi mdi-arrow-right ms-1 align-middle"></i></a>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="tab-pane" id="profile1" role="tabpanel">
                                                                <p class="mb-0"></p>
                                                            </div>
                                                            <div class="tab-pane" id="messages1" role="tabpanel">
                                                                <p class="mb-0">
                                                                    Etsy mixtape wayfarers, ethical wes anderson tofu
                                                                    before they
                                                                    sold out mcsweeney's organic lomo retro fanny pack
                                                                    lo-fi
                                                                    farm-to-table readymade. Messenger bag gentrify
                                                                    pitchfork
                                                                    tattooed craft beer, iphone skateboard locavore
                                                                    carles etsy
                                                                    salvia banksy hoodie helvetica. DIY synth PBR banksy
                                                                    irony.
                                                                    Leggings gentrify squid 8-bit cred pitchfork.
                                                                    Williamsburg banh
                                                                    mi whatever gluten-free carles.
                                                                </p>
                                                            </div>
                                                            <div class="tab-pane" id="settings1" role="tabpanel">
                                                                <p class="mb-0">
                                                                    Trust fund seitan letterpress, keytar raw denim
                                                                    keffiyeh etsy
                                                                    art party before they sold out master cleanse
                                                                    gluten-free squid
                                                                    scenester freegan cosby sweater. Fanny pack portland
                                                                    seitan DIY,
                                                                    art party locavore wolf cliche high life echo park
                                                                    Austin. Cred
                                                                    vinyl keffiyeh DIY salvia PBR, banh mi before they
                                                                    sold out
                                                                    farm-to-table VHS viral locavore cosby sweater. Lomo
                                                                    wolf viral,
                                                                    mustache readymade keffiyeh craft.
                                                                </p>
                                                            </div>
                                                        </div>
                                                    </div><!-- end card-body -->
                                                </div><!-- end card -->
                                            </div>
                                        </div>
                                        <div class="tab-pane" id="SearchFunds" role="tabpanel">
                                            <p class="mb-0">
                                            <div class="card-body">
                                                <form class="needs-validation" novalidate>
                                                    <div class="row">
                                                        <div class="col-md-4">
                                                            <div class="mb-3 position-relative">
                                                                <label class="form-label"
                                                                    for="validationTooltip01">Category</label>
                                                                <select class="form-select">
                                                                    <option>Select</option>
                                                                    <option>Large select</option>
                                                                    <option>Small select</option>
                                                                </select>
                                                                <div class="valid-tooltip">
                                                                    Looks good!
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <div class="mb-3 position-relative">
                                                                <label class="form-label"
                                                                    for="validationTooltip02">Sub-Category</label>
                                                                <select class="form-select">
                                                                    <option>Select</option>
                                                                    <option>Large select</option>
                                                                    <option>Small select</option>
                                                                </select>
                                                                <div class="valid-tooltip">
                                                                    Looks good!
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <div class="mb-3 position-relative">
                                                                <label class="form-label"
                                                                    for="validationTooltip02">Type</label>
                                                                <select class="form-select">
                                                                    <option>Select</option>
                                                                    <option>Large select</option>
                                                                    <option>Small select</option>
                                                                </select>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-4">
                                                            <div class="mb-3 position-relative">
                                                                <label class="form-label"
                                                                    for="validationTooltip03">Option
                                                                </label>
                                                                <div class="row">
                                                                    <div class="col-md-6">
                                                                        <div class="form-check mb-2">
                                                                            <input class="form-check-input" type="radio"
                                                                                name="formRadios" id="formRadios1"
                                                                                checked>
                                                                            <label class="form-check-label"
                                                                                for="formRadios1">
                                                                                Growth
                                                                            </label>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-6 text-center">
                                                                        <div class="form-check mb-2">
                                                                            <input class="form-check-input" type="radio"
                                                                                name="formRadios" id="formRadios1"
                                                                                checked>
                                                                            <label class="form-check-label"
                                                                                for="formRadios1">
                                                                                Dividend
                                                                            </label>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="invalid-tooltip">
                                                                    Please provide a valid city.
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <div class="mb-3 position-relative">
                                                                <label class="form-label"
                                                                    for="validationTooltip02">Period</label>
                                                                <select class="form-select">
                                                                    <option>Select</option>
                                                                    <option>Large select</option>
                                                                    <option>Small select</option>
                                                                </select>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <div class="mb-3 position-relative">
                                                                <label class="form-label"
                                                                    for="validationTooltip02">Rank</label>
                                                                <select class="form-select">
                                                                    <option>Select</option>
                                                                    <option>Large select</option>
                                                                    <option>Small select</option>
                                                                </select>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-4">
                                                            <div class="mb-3 position-relative">
                                                                <label class="form-label"
                                                                    for="validationTooltip03">Minimum Investment
                                                                </label>
                                                                <div class="row">
                                                                    <div class="col-md-12" style="padding-left: 10px;">
                                                                        <div class="form-check mb-2"
                                                                            style="padding-left: 0px;">
                                                                            <div id="slider"></div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="invalid-tooltip">
                                                                    Please provide a valid city.
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <div class="mb-3 position-relative">
                                                                <label class="form-label"
                                                                    for="validationTooltip03">Minimum Investment
                                                                </label>
                                                                <div class="row">
                                                                    <div class="col-md-12" style="padding-left: 10px;">
                                                                        <div class="form-check mb-2"
                                                                            style="padding-left: 0px;">
                                                                            <div id="slider1"></div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="invalid-tooltip">
                                                                    Please provide a valid city.
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <div class="mb-3 position-relative">
                                                                <label class="form-label" for="validationTooltip02">Fund
                                                                    Risk</label>
                                                                <div class="">
                                                                    <div class="btn-group btn-group-example btn-group-sm mb-3"
                                                                        role="group">

                                                                        <button type="button"
                                                                            class="btn btn-primary btn-success-soft">Low</button>
                                                                        <button type="button"
                                                                            class="btn btn-success w-xs">Mid
                                                                            low</button>
                                                                        <button type="button"
                                                                            class="btn btn-info w-xs">Mod</button>
                                                                        <button type="button"
                                                                            class="btn btn-warning w-xs">Mod
                                                                            High</button>
                                                                        <button type="button"
                                                                            class="btn btn-danger w-xs">High</button>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-sm-12 mt-2"
                                                        style="text-align: right; padding: 0px;">
                                                        <a href="" class="btn btn-primary"> Search</a>
                                                    </div>
                                                </form>
                                            </div>
                                            <div class="card">
                                                <div class="card-body pb-xl-2">
                                                    <h4 class="font-size-20 mb-1"><a href="" class="text-dark">Axis
                                                            Growth
                                                            Opportunities Fund-Growth
                                                        </a></h4>
                                                    <div class="col-xl-12">

                                                        <div class="row">
                                                            <div class="col-xl-4">
                                                                <div class="col-xl-12">
                                                                    <p>
                                                                        12.953 <span
                                                                            class="mdi mdi-arrow-up-bold text-success"></span>
                                                                        + .0156 ( +1.19%)
                                                                    </p>
                                                                </div>
                                                                <div class="col-xl-12">
                                                                    <div class="row">
                                                                        <div class="col">
                                                                            <div>
                                                                                <h6 style="margin-bottom: 0px;">
                                                                                    13.45 <small class="text-muted">(20
                                                                                        Jan
                                                                                        2023)</small>
                                                                                </h6>
                                                                                <p class="text-muted mb-0">
                                                                                    <span
                                                                                        class="badge badge-soft-success font-size-12">High
                                                                                        Nav</span>
                                                                                </p>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col">
                                                                            <div>
                                                                                <h6 style="margin-bottom: 0px;">
                                                                                    11.23 <small class="text-muted">(28
                                                                                        Oct
                                                                                        2023)</small>
                                                                                </h6>
                                                                                <p class="text-muted mb-0">
                                                                                    <span
                                                                                        class="badge badge-soft-danger font-size-12">Low
                                                                                        Nav</span>
                                                                                </p>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div class="col-xl-8">
                                                                <div class="row">
                                                                    <div class="col border-end">
                                                                        <div class="text-center">
                                                                            <p class="text-muted mb-0">
                                                                                6 months</p>
                                                                            <h6>12.09%</h6>

                                                                        </div>
                                                                    </div>
                                                                    <div class="col border-end">
                                                                        <div class="text-center">
                                                                            <p class="text-muted mb-0">
                                                                                1
                                                                                year</p>
                                                                            <h6>14.52%</h6>

                                                                        </div>
                                                                    </div>
                                                                    <div class="col border-end">
                                                                        <div class="text-center">
                                                                            <p class="text-muted mb-0">
                                                                                3
                                                                                years</p>
                                                                            <h6>24.82%</h6>

                                                                        </div>
                                                                    </div>
                                                                    <div class="col border-end">
                                                                        <div class="text-center">
                                                                            <p class="text-muted mb-0">
                                                                                Since inception</p>
                                                                            <h6>16.78%</h6>

                                                                        </div>
                                                                    </div>
                                                                    <div class="col">
                                                                        <div class="text-center">
                                                                            <p class="text-muted mb-0">
                                                                                Fund size (Cr)</p>
                                                                            <h6>2,324.14</h6>

                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="row">
                                                                    <div class="col-sm-2 mt-2">
                                                                        <p class="mt-4 mb-0" style="text-align: right;">
                                                                            18.09% </p>
                                                                    </div>
                                                                    <div class="col-xl-8 mt-2">
                                                                        <div class="row align-items-center g-0">
                                                                            <div class="col-sm-12 mt-2">
                                                                                <p class="text-muted mb-0">
                                                                                    Average Category
                                                                                    Returns(3 yrs)
                                                                                </p>
                                                                            </div>

                                                                            <div class="col-sm-12">
                                                                                <div class="progress mt-1"
                                                                                    style="height: 6px;">
                                                                                    <div class="progress-bar progress-bar bg-primary"
                                                                                        role="progressbar"
                                                                                        style="width: 72%"
                                                                                        aria-valuenow="52"
                                                                                        aria-valuemin="0"
                                                                                        aria-valuemax="52">
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-sm-2 mt-2">
                                                                        <p class="mt-4 mb-0" style="text-align: left;">
                                                                            27.56%</p>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                        </div>
                                                        <div class="col-sm-12 mt-2"
                                                            style="text-align: right; padding: 0px;">
                                                            <a href="" class="text-primary fw-semibold"> <u>More details</u>
                                                                <i class="mdi mdi-arrow-right ms-1 align-middle"></i></a>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            </p>
                                        </div>
                                        <div class="tab-pane" id="CompareFunds" role="tabpanel">
                                            <div class="mb-3">
                                                <div class="row">
                                                    <div class="col-lg-3 col-sm-6">
                                                        <div data-bs-toggle="collapse">
                                                            <label class="card-radio-label mb-0">
                                                                <span class="card-radio text-truncate">
                                                                    <div class="text-center mt-2 mb-2">
                                                                        <button type="button" class="btn btn-success waves-effect rounded-circle"
                                                                            data-bs-toggle="modal" data-bs-target=".bs-example-modal-lg"><i
                                                                                class="mdi mdi-plus"></i>
                                                                        </button>
                                                                        <h6 class="mt-2">Add Scheme</h6>
                                                                        <!--  Large modal example -->
                                                                        <div class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog"
                                                                            aria-labelledby="myLargeModalLabel" aria-hidden="true">
                                                                            <div class="modal-dialog modal-lg">
                                                                                <div class="modal-content">
                                                                                    <div class="modal-header">
                                                                                        <h5 class="modal-title" id="myLargeModalLabel">Add Scheme</h5>
                                                                                        <button type="button" class="btn-close" data-bs-dismiss="modal"
                                                                                            aria-label="Close"></button>
                                                                                    </div>
                                                                                    <div class="modal-body">
                                                                                        <form class="needs-validation" novalidate>
                                                                                            <div class="row" style="text-align: left!important;">
                                                                                                <div class="col-md-4">
                                                                                                    <div class="mb-3 position-relative">
                                                                                                        <label class="form-label" for="validationTooltip01">Mutual
                                                                                                            Funds</label>
                                                                                                        <select class="form-select">
                                                                                                            <option> Select</option>
                                                                                                            <option> Large select</option>
                                                                                                            <option> Small select</option>
                                                                                                        </select>
                                                                                                        <div class="valid-tooltip">  Looks good! </div>
                                                                                                    </div>
                                                                                                </div>
                                                                                                <div class="col-md-4">
                                                                                                    <div class="mb-3 position-relative">
                                                                                                        <label class="form-label" for="validationTooltip01">Category</label>
                                                                                                        <select
                                                                                                            class="form-select">
                                                                                                            <option> Select </option>
                                                                                                            <option> Large select </option>
                                                                                                            <option> Small select </option>
                                                                                                        </select>
                                                                                                        <div class="valid-tooltip"> Looks good! </div>
                                                                                                    </div>
                                                                                                </div>
                                                                                                <div class="col-md-4">
                                                                                                    <div class="mb-3 position-relative">
                                                                                                        <label class="form-label" for="validationTooltip02">Sub-Category</label>
                                                                                                        <select class="form-select">
                                                                                                            <option> Select </option>
                                                                                                            <option> Large select </option>
                                                                                                            <option> Small select </option>
                                                                                                        </select>
                                                                                                        <div class="valid-tooltip"> Looks good! </div>
                                                                                                    </div>
                                                                                                </div>

                                                                                            </div>
                                                                                            <div class="row"
                                                                                                style="text-align: left!important;">
                                                                                                <div class="col-md-4">
                                                                                                    <div
                                                                                                        class="mb-3 position-relative">
                                                                                                        <label
                                                                                                            class="form-label"
                                                                                                            for="validationTooltip02">Type</label>
                                                                                                        <select
                                                                                                            class="form-select">
                                                                                                            <option>
                                                                                                                Select
                                                                                                            </option>
                                                                                                            <option>
                                                                                                                Large
                                                                                                                select
                                                                                                            </option>
                                                                                                            <option>
                                                                                                                Small
                                                                                                                select
                                                                                                            </option>
                                                                                                        </select>
                                                                                                    </div>
                                                                                                </div>
                                                                                                <div class="col-md-4">
                                                                                                    <div
                                                                                                        class="mb-3 position-relative">
                                                                                                        <label
                                                                                                            class="form-label"
                                                                                                            for="validationTooltip03">Option
                                                                                                        </label>
                                                                                                        <div
                                                                                                            class="row">
                                                                                                            <div
                                                                                                                class="col-md-6">
                                                                                                                <div
                                                                                                                    class="form-check mb-2">
                                                                                                                    <input
                                                                                                                        class="form-check-input"
                                                                                                                        type="radio"
                                                                                                                        name="formRadios"
                                                                                                                        id="formRadios1"
                                                                                                                        checked>
                                                                                                                    <label
                                                                                                                        class="form-check-label"
                                                                                                                        for="formRadios1">
                                                                                                                        Growth
                                                                                                                    </label>
                                                                                                                </div>
                                                                                                            </div>
                                                                                                            <div
                                                                                                                class="col-md-6 text-center">
                                                                                                                <div
                                                                                                                    class="form-check mb-2">
                                                                                                                    <input
                                                                                                                        class="form-check-input"
                                                                                                                        type="radio"
                                                                                                                        name="formRadios"
                                                                                                                        id="formRadios1"
                                                                                                                        checked>
                                                                                                                    <label
                                                                                                                        class="form-check-label"
                                                                                                                        for="formRadios1">
                                                                                                                        Dividend
                                                                                                                    </label>
                                                                                                                </div>
                                                                                                            </div>
                                                                                                        </div>

                                                                                                        <div
                                                                                                            class="invalid-tooltip">
                                                                                                            Please
                                                                                                            provide a
                                                                                                            valid city.
                                                                                                        </div>
                                                                                                    </div>
                                                                                                </div>
                                                                                                <div class="col-md-4">
                                                                                                    <div
                                                                                                        class="mb-3 position-relative">
                                                                                                        <label
                                                                                                            class="form-label"
                                                                                                            for="validationTooltip02">Choose
                                                                                                            Scheme</label>
                                                                                                        <select
                                                                                                            class="form-select">
                                                                                                            <option>
                                                                                                                Select
                                                                                                            </option>
                                                                                                            <option>
                                                                                                                Large
                                                                                                                select
                                                                                                            </option>
                                                                                                            <option>
                                                                                                                Small
                                                                                                                select
                                                                                                            </option>
                                                                                                        </select>
                                                                                                    </div>
                                                                                                </div>

                                                                                            </div>
                                                                                            <div class="row"
                                                                                                style="text-align: left!important;">
                                                                                                <div class="col-md-4">
                                                                                                    <div
                                                                                                        class="mb-3 position-relative">
                                                                                                        <label
                                                                                                            class="form-label"
                                                                                                            for="validationTooltip02">Index</label>
                                                                                                        <select
                                                                                                            class="form-select">
                                                                                                            <option>
                                                                                                                Select
                                                                                                            </option>
                                                                                                            <option>
                                                                                                                Large
                                                                                                                select
                                                                                                            </option>
                                                                                                            <option>
                                                                                                                Small
                                                                                                                select
                                                                                                            </option>
                                                                                                        </select>
                                                                                                    </div>
                                                                                                </div>
                                                                                                <div class="col-md-4">
                                                                                                </div>


                                                                                                <div class="col-md-4"
                                                                                                    style="text-align: right;">
                                                                                                    <label
                                                                                                        class="form-label"
                                                                                                        for="validationTooltip02">&nbsp;</label>
                                                                                                    <div
                                                                                                        class="mt-1 position-relative">
                                                                                                        <a href="javascript:void(0);"
                                                                                                            data-bs-toggle="modal"
                                                                                                            data-bs-target=".add-new"
                                                                                                            class="btn btn-success btn-md waves-effect waves-light">Add</a>
                                                                                                    </div>
                                                                                                </div>
                                                                                            </div>
                                                                                        </form>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <!--  Large modal example -->
                                                                    </div>
                                                                </span>
                                                            </label>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-3 col-sm-6">
                                                        <div data-bs-toggle="collapse">
                                                            <label class="card-radio-label mb-0">
                                                                <div class="card-radio p-2">
                                                                    <div class="text-center mt-2 mb-2" style="white-space: normal;">
                                                                        <h6>Aditya Birla Sun Life PSU Equity Fund Direct - Growth</h6>
                                                                    </div>
                                                                    <div class="edit-btn rounded">
                                                                        <a href="#" data-bs-toggle="tooltip"
                                                                            data-placement="top" title=""
                                                                            data-bs-original-title="delete"
                                                                            class="text-danger bg-light">
                                                                            <i class="mdi mdi-delete-circle font-size-16 mt-2"></i>
                                                                        </a>
                                                                    </div>
                                                                </div>
                                                                
                                                            </label>
                                                            
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-3 col-sm-6">
                                                        <div data-bs-toggle="collapse">
                                                            <label class="card-radio-label mb-0">
                                                                <span class="card-radio text-truncate">
                                                                    <div class="text-center mt-2 mb-2">
                                                                        <button type="button" class="btn btn-success waves-effect rounded-circle"
                                                                            data-bs-toggle="modal" data-bs-target=".bs-example-modal-lg"><i
                                                                                class="mdi mdi-plus"></i>
                                                                        </button>
                                                                        <h6 class="mt-2">Add Scheme</h6>
                                                                    </div>
                                                                </span>
                                                            </label>
                                                            <!-- <div class="edit-btn bg-light rounded">
                                                                <a href="#"  data-bs-toggle="tooltip" data-placement="top" title="" data-bs-original-title="Edit">
                                                                    <i class="bx bx-pencil font-size-16"></i>
                                                                </a>
                                                            </div> -->
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-3 col-sm-6">
                                                        <div data-bs-toggle="collapse">
                                                            <label class="card-radio-label mb-0">
                                                                <span class="card-radio text-truncate">
                                                                    <div class="text-center mt-2 mb-2">
                                                                        <button type="button" class="btn btn-success waves-effect rounded-circle"
                                                                        data-bs-toggle="modal" data-bs-target=".bs-example-modal-lg"><i
                                                                            class="mdi mdi-plus"></i>
                                                                    </button>
                                                                    <h6 class="mt-2">Add Scheme</h6>
                                                                    </div>
                                                                </span>
                                                            </label>
                                                            <!-- <div class="edit-btn bg-light rounded">
                                                                <a href="#"  data-bs-toggle="tooltip" data-placement="top" title="" data-bs-original-title="Edit">
                                                                    <i class="bx bx-pencil font-size-16"></i>
                                                                </a>
                                                            </div> -->
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-sm-12 mt-2 mb-3"
                                                        style="text-align: right;">
                                                        <a href="" class="btn btn-primary">Show Performance
                                                        </a>
                                                    </div>
                                                </div>
                                                <div class="row mt-2">
                                                    <div class="col-sm-12">
                                                        <div class="card">
                                                            <div class="card-body pb-xl-2">
                                                                <div class="table-responsive">
                                                                    <table class="table align-middle table-nowrap table-centered mb-0">
                                                                        <thead>
                                                                            <tr>
                                                                                <th class="fw-bold">Scheme Name</th>
                                                                                <th class="fw-bold" style="text-align: right;">6 months.</th>
                                                                                <th class="fw-bold" style="text-align: right;">1 year</th>
                                                                                <th class="fw-bold" style="text-align: right;">3 years</th>
                                                                                <th class="fw-bold" style="text-align: right;">Since inception</th>
                                                                                <th class="fw-bold" style="text-align: right;">Fund size (Cr) </th>
                                                                            </tr>
                                                                        </thead><!-- end thead -->
                                                                        <tbody>
                                                                            <tr>
                                                                                <td>
                                                                                    <div>
                                                                                        <h5 class="text-truncate font-size-14 mb-1">Kotak Mutual Multicap Fund-Growth
                                                                                        </h5>
                                                                                        12.953 <span
                                                                                        class="mdi mdi-arrow-up-bold text-success"></span>
                                                                                    + .0156 ( +1.19%)
                                                                                    </div>
                                                                                </td>
                                                                                <th style="text-align: right;">12.09%</th>
                                                                                <td style="text-align: right;">14.52%</td>
                                                                                <td style="text-align: right;">24.82%</td>
                                                                                <td style="text-align: right;">16.78%</td>
                                                                                <td style="text-align: right;">2,324.14</td>
                                                                            </tr>
                                                                            <!-- end tr -->
                                                                            <tr>
                                                                                <td>
                                                                                    <div>
                                                                                        <h5 class="text-truncate font-size-14 mb-1">Axis Growth Opportunities Fund-Growth
                                                                                        </h5>
                                                                                        12.953 <span
                                                                                        class="mdi mdi-arrow-up-bold text-success"></span>
                                                                                    + .0156 ( +1.19%)
                                                                                    </div>
                                                                                </td>
                                                                                <th style="text-align: right;">12.09%</th>
                                                                                <td style="text-align: right;">14.52%</td>
                                                                                <td style="text-align: right;">24.82%</td>
                                                                                <td style="text-align: right;">16.78%</td>
                                                                                <td style="text-align: right;">2,324.14</td>
                                                                            </tr>
                                                                           
                                                                        </tbody><!-- end tbody -->
                                                                    </table><!-- end table -->
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                   
                                                </div>
                                            </div>
                                        </div>
                                        <div class="tab-pane" id="NavTraker" role="tabpanel">
                                            <div class="card-body">
                                                <form class="needs-validation" novalidate>
                                                    <div class="row">
                                                        <div class="col-md-4">
                                                            <div class="mb-3 position-relative">
                                                                <label class="form-label"
                                                                    for="validationTooltip01">Mutual Funds</label>
                                                                <select class="form-select">
                                                                    <option>Select</option>
                                                                    <option>Large select</option>
                                                                    <option>Small select</option>
                                                                </select>
                                                                <div class="valid-tooltip">
                                                                    Looks good!
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <div class="mb-3 position-relative">
                                                                <label class="form-label"
                                                                    for="validationTooltip01">Category</label>
                                                                <select class="form-select">
                                                                    <option>Select</option>
                                                                    <option>Large select</option>
                                                                    <option>Small select</option>
                                                                </select>
                                                                <div class="valid-tooltip">
                                                                    Looks good!
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <div class="mb-3 position-relative">
                                                                <label class="form-label"
                                                                    for="validationTooltip02">Sub-Category</label>
                                                                <select class="form-select">
                                                                    <option>Select</option>
                                                                    <option>Large select</option>
                                                                    <option>Small select</option>
                                                                </select>
                                                                <div class="valid-tooltip">
                                                                    Looks good!
                                                                </div>
                                                            </div>
                                                        </div>

                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-4">
                                                            <div class="mb-3 position-relative">
                                                                <label class="form-label"
                                                                    for="validationTooltip02">Type</label>
                                                                <select class="form-select">
                                                                    <option>Select</option>
                                                                    <option>Large select</option>
                                                                    <option>Small select</option>
                                                                </select>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <div class="mb-3 position-relative">
                                                                <label class="form-label"
                                                                    for="validationTooltip03">Option
                                                                </label>
                                                                <div class="row">
                                                                    <div class="col-md-6">
                                                                        <div class="form-check mb-2">
                                                                            <input class="form-check-input" type="radio"
                                                                                name="formRadios" id="formRadios1"
                                                                                checked>
                                                                            <label class="form-check-label"
                                                                                for="formRadios1">
                                                                                Growth
                                                                            </label>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-6 text-center">
                                                                        <div class="form-check mb-2">
                                                                            <input class="form-check-input" type="radio"
                                                                                name="formRadios" id="formRadios1"
                                                                                checked>
                                                                            <label class="form-check-label"
                                                                                for="formRadios1">
                                                                                Dividend
                                                                            </label>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="invalid-tooltip">
                                                                    Please provide a valid city.
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <div class="mb-3 position-relative">
                                                                <label class="form-label"
                                                                    for="validationTooltip02">Choose Scheme</label>
                                                                <select class="form-select">
                                                                    <option>Select</option>
                                                                    <option>Large select</option>
                                                                    <option>Small select</option>
                                                                </select>
                                                            </div>
                                                        </div>

                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-4">
                                                            <div class="mb-3 position-relative">
                                                                <label class="form-label"
                                                                    for="validationTooltip02">Index</label>
                                                                <select class="form-select">
                                                                    <option>Select</option>
                                                                    <option>Large select</option>
                                                                    <option>Small select</option>
                                                                </select>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <label class="form-label"
                                                                for="validationTooltip02">&nbsp;</label>
                                                            <div class="mt-1 position-relative">
                                                                <a href="javascript:void(0);" data-bs-toggle="modal"
                                                                    data-bs-target=".add-new"
                                                                    class="btn btn-success btn-sm waves-effect waves-light"><i
                                                                        class="mdi mdi-plus"></i>Add</a>
                                                            </div>
                                                        </div>

                                                        <div class="col-md-4">

                                                        </div>
                                                    </div>
                                                </form>
                                            </div>
                                        </div>
                                    </div>
                                </div><!-- end card-body -->
                            </div><!-- end card -->
                        </div><!-- end col -->
                    </div><!-- end row -->


                </div>
                <!-- container-fluid -->
            </div>
            <!-- End Page-content -->


        </div>
        <!-- end main content-->

    </div>
    <!-- END layout-wrapper -->


 <!-- JAVASCRIPT -->
 <script src="assets/libs/bootstrap/js/bootstrap.bundle.min.js"></script>
 <script src="assets/libs/metismenujs/metismenujs.min.js"></script>
 <script src="assets/libs/simplebar/simplebar.min.js"></script>
 <script src="assets/libs/feather-icons/feather.min.js"></script>

 <script src="assets/js/app.js"></script>

 <!-- nouisliderribute js -->
 <script src="assets/libs/nouislider/nouislider.min.js"></script>
 <script src="assets/libs/wnumb/wNumb.min.js"></script>

 <!-- range slider init -->
 <script src="assets/js/range-sliders.init.js"></script>


</body>

</html>
