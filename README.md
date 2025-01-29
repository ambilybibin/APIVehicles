Created 4 end points as below

1. Get list of all cars available "/Vehicles/Cars"

2. Get list of all cars available by make "/Vehicles/Cars/Make?make={make}"

3. Get list of all cars available by model "/Vehicles/Cars/Model?model={model}

4. Get list of all cars available between a range of mileage "Vehicles/Search/Mileage?minMileage={minMileage}&maxMileage={maxMileage}"

Also written 7 test cases as below

1. TestGetAllVehicles()
2. GetCarsByMileageRange_ReturnsFilteredCars_WhenValidMileageRangeIsProvided
3. GetCarsByMileageRange_ReturnsBadRequest_WhenMinMileageIsGreaterThanMaxMileage
4. GetCarsByMileageRange_ReturnsBadRequest_WhenMileageRangeIsMissing
5. GetAllCarsListByModel_ReturnsSuccessWithCorrectData
6. GetAllCarsListByModel_ShouldReturnCorrectCars_IgnoringCase
7. GetAllCarsListByModel_ShouldReturnAllCars_WhenNoModelProvided



