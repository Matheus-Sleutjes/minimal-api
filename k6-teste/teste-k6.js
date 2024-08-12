import http from 'k6/http';

export const options = {
    stages: [
        { duration: '30s', target: 20 },
        { duration: '9m00s', target: 10 },
        { duration: '30s', target: 0 },
    ],
};

// export default function SQLPost() {
//     let body = {
//         id: 0,
//         descricao: "SQL"
//     }

//     const res = http.post('http://localhost:5072/SQLPost', JSON.stringify(body), {
//         headers: { 'Content-Type': 'application/json' }
//     });
// }

// export default function EFPost() {
//     let body = {
//         id: 0,
//         descricao: "EF"
//     }

//     const res = http.post('http://localhost:5072/EFPost', JSON.stringify(body), {
//         headers: { 'Content-Type': 'application/json' }
//     });
// }

// export default function SQLGet() {
//     const res = http.get('http://localhost:5072/SQLGet/1000000', {
//         headers: { 'Content-Type': 'application/json' }
//     });
// }

// export default function EFGet() {
//     const res = http.get('http://localhost:5072/EFGet/1000000', {
//         headers: { 'Content-Type': 'application/json' }
//     });
// }