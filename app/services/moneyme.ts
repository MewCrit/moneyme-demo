const BaseURL = process.env.NEXT_PUBLIC_MONEYME_URL; 

export const createLoan = async (data: any) => {
    try {

        console.log(JSON.stringify(data))
        const response = await fetch(`${BaseURL}/v1/moneyme/loans`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const responseData = await response.json();
        return responseData;
    } catch (error) {
        console.error('Error posting data:', error);
        throw error;
    }
};


export const getLoanByUserID = async (id : string) => {
    try {
        const response = await fetch(`${BaseURL}/v1/moneyme/loans/${id}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            },
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const result = await response.json()
        return result
    }
    catch(error) {
        console.error('Error retrieving data:', error);
        throw error;
    }
}



export const calculateLoan = async (data: any) => {
    try {

        const response = await fetch(`${BaseURL}/v1/moneyme/loans/calculations`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const responseData = await response.json();
        return responseData;
    } catch (error) {
        console.error('Error posting data:', error);
        throw error;
    }

};

export const updateLoan = async(data : any, id : string) => {

    try {

        console.log(JSON.stringify(data))
        const response = await fetch(`${BaseURL}/v1/moneyme/loans/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const responseData = await response.json();
        return responseData;
    } catch (error) {
        console.error('Error posting data:', error);
        throw error;
    }
}


export const createFinalLoan = async ( data : any, id : string) => {

    try {

        console.log(JSON.stringify(data))
        const response = await fetch(`${BaseURL}/v1/moneyme/loans/${id}/apply`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const responseData = await response.json();
        return responseData;
    } catch (error) {
        console.error('Error posting data:', error);
        throw error;
    }

}