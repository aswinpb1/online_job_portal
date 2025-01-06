function date()
{
    const a = document.getElementById('dob').value;
    const currentdate = new Date();

    const dob = new Date(a);

    let dateofbirth = currentdate.getFullYear() - dob.getFullYear();
    if (dateofbirth < 0) {
        document.getElementById('dateerror').innerHTML = "Date of Birth cannot be today or a future date.";
        return;
    }
    else
    {
        document.getElementById('dateerror').innerHTML = "";
    }
}

function phonevalidation()
{
    const a = document.getElementById('phone').value;
    const phoneRegex = /^[9876]/;

    if (!phoneRegex.test(a)) {
        document.getElementById('numbererror').innerHTML = "Phone number must start with 9, 8,7 or 6.";
        return;
    }
    else
    {
        document.getElementById('numbererror').innerHTML = "";
    }
}