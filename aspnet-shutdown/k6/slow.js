import { check } from 'k6';
import http from 'k6/http';

export const options = {
  thresholds: {
    http_req_failed: ['rate<0.001']
  },
};

export default function () {
  try {
    const res = http.get('http://localhost:80/slow');

    // Log all responses
    console.log(`endpoint=slow status=${res.status}`);

    // Log failures explicitly
    if (res.status !== 200) {
      console.error(`endpoint=slow status=${res.status} body=${res.body}`);
    }

    if (res.error) {
      console.error(`endpoint=slow network_error=${res.error}`);
    }

    check(res, {
      'response code was 200': (r) => r.status === 200,
    });

  } catch (err) {
    console.error(`endpoint=slow error=${err.message}`);
  }
}