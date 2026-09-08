import http from 'k6/http';
import { check, sleep } from 'k6';

// Test settings: 10 users, running for 30 seconds
export const options = {
    vus: 10,        // number of virtual users
    duration: '30s', // duration of load test
};

// This function repeats for each virtual user
export default function () {

    // User 1: hit the products endpoint
    let productsRes = http.get('https://localhost:4000/api/Products', {
        insecureSkipTLSVerify: true, // ignore SSL warning on localhost
    });
    // Check the response was successful
    check(productsRes, {
        'products status 200': (r) => r.status === 200,
    });

    sleep(1); // wait 1 second before next request

    // User 2: hit the available products endpoint
    let availableRes = http.get('https://localhost:4000/api/Products/available', {
        insecureSkipTLSVerify: true,
    });
    check(availableRes, {
        'available status 200': (r) => r.status === 200,
    });

    sleep(1);

    // User 3: hit the orders endpoint
    let ordersRes = http.get('https://localhost:4000/api/Orders', {
        insecureSkipTLSVerify: true,
    });
    check(ordersRes, {
        'orders status 200': (r) => r.status === 200,
    });

    sleep(1);
}