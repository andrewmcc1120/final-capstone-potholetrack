// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


/**
 * Saves the location of a map marker and the severity selected as a pothole report
 * and gives it a unique Report Number
 * @returns {void}
 */
function saveData() {
    let reportNumber = getRndInteger();
    // The start of the url for our api call
    let base = window.location.protocol + "//" + window.location.host;
    let url = `${base}/api/record/create`;
    let lat = marker.getPosition().lat();
    let lng = marker.getPosition().lng();
    //Gets the severity of the pothole the user selected
    let severity = parseInt(document.querySelector('#marker-severity').selectedOptions[0].value);
    if (lng >= -81.820158 &&
        lng <= -81.535962 &&
        lat <= 41.579773 &&
        lat >= 41.427390) {

        let settings = {
            method: 'POST',
            credentials: 'include',
            body: JSON.stringify({
                Lattitude: lat,
                Longitude: lng,
                Severity: severity,
                Status: 1,
                ReportNumber: 'CLE' + reportNumber
            }),
            headers: {
                'Content-Type': 'application/json'
            }
        };

        //Api call
        fetch(url, settings)
            .then(function (response) {
                if (!response.ok) {
                    console.log(response.statusText);
                    //throw Error(response.statusText);
                }
                return response;
            }).then(function (response) {
                console.log("ok");
            }).catch(error => {
                console.error('Error:', error);
            });
    }

}

/**
 *  Gets a psuedo-random number to save as the Report Number
 * @returns {number}    a psuedo-random number
 */
function getRndInteger() {
    return Math.floor(Math.random() * (80000 - 35000 + 1)) + 35000;
}

//Logs the current user out when they click the log out button
let button = $('#logout');
button.on('click', () => {
    document.querySelector('form[name="logout"]').submit();
});

/**
 * Gets the record id from a pothole marker and adds another user report
 *  @returns {void}
 */
function incrementReportCount() {
    let recordId = $('form[name="updateRecord"]').children('#p-id').val();
        // The start of the url for our api call
    let base = window.location.protocol + "//" + window.location.host;
    let url = `${base}/api/record/AddReport`;
    let settings = {
        method: 'POST',
        credentials: 'include',
        body: JSON.stringify({
            Id: recordId
        }),
        headers: {
            'Content-Type': 'application/json'
        }
    };

    //Api call
    fetch(url, settings)
        .then(function (response) {
            if (!response.ok) {
                console.log(response.statusText);
                //throw Error(response.statusText);
            }
            return response;
        }).then(function (response) {
            console.log("ok");
        }).catch(error => {
            console.error('Error:', error);
        });
}

/**
 * Updates the information associated with an existing pothole record 
 * @returns {void}
 */
function updateReport() {

        // The start of the url for our api call
    let base = window.location.protocol + "//" + window.location.host;
    let id = $('#report-id', $('#employee-modal')).val();
    let url = `${base}/api/record/update`;
    let dateInspected = $('#dateinspected', $('#employee-modal')).val();
    let dateRepaired = $('#daterepaired', $('#employee-modal')).val();
    let description = $('#description', $('#employee-modal')).val();
    let status = $('#status', $('#employee-modal')).val();
    let severity = $('#severity', $('#employee-modal')).val();
    //Yesterday's date for handling when no dates are passed into the report info modal
    let defaultDate = new Date();
    defaultDate.setDate(defaultDate.getDate() - 1);
    if (dateInspected === "") {
        dateInspected = defaultDate;
    }
    if (dateRepaired === "") {
        dateRepaired = defaultDate;
    }

    //Today's date for handling when dates are set to today's date
    let thisDay = defaultDate.getDate() +1;
    let thisYr = defaultDate.getFullYear();
    let thisMonth = defaultDate.getMonth() + 1;
    if (thisDay < 10) {
        thisDay = `0${thisDay}`;
    }
    if (thisMonth < 10) {
        thisMonth = `0${thisMonth}`;
    }
    let todaysDate = `${thisYr}-${thisMonth}-${thisDay}`;
    if (dateInspected === todaysDate) {
        dateInspected = new Date();
    }
    if (dateRepaired === todaysDate) {
        dateRepaired = new Date();
    }

    let settings = {
        method: 'POST',
        credentials: 'include',
        body: JSON.stringify({
            //Only passing in values that are being changed and the id so that we
            // can get the existing report when updating.
            // Also passing in anything marked as required in the model
            // Otherwise we get a 400 error
            Id: id,
            Severity: severity,
            Status: status,
            Description: description,
            DateInspected: dateInspected,
            DateRepaired: dateRepaired
        }),
        headers: {
            'Content-Type': 'application/json'
        }
    };

    //Api call
    fetch(url, settings)
        .then(function (response) {
            console.log(response.headers, response.url);
            if (!response.ok) {
                console.log(response.statusText);
                //throw Error(response.statusText);
            }
            return response;
        }).then(function (response) {
            console.log("ok");
        }).catch(error => {
            console.error('Error:', error);
        });


}

/**
 * Assigns an employee to a pothole record
 * @param {number} reportId The id of the report being assigned
 * @returns {void}
 */
function assignEmployee(reportId) {

        // The start of the url for our api call
    var base = window.location.protocol + "//" + window.location.host;
    var id = reportId;
    var url = `${base}/api/record/assign/${id}`;

    //No body being sent because the report id is in the url
    var settings = {
        method: 'POST',
        credentials: 'include',
        headers: {
            'Content-Type': 'application/json'
        }
    };

    //Api call
    fetch(url, settings)
        .then(function (response) {
            if (!response.ok) {
                console.log(response.statusText);
                //throw Error(response.statusText);
            }
            return response;
        }).then(function (response) {
            console.log("ok");
        }).catch(error => {
            console.error('Error:', error);
        });
}
