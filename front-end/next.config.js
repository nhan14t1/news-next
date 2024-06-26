const config = {
  publicRuntimeConfig: {
    // Will be available on both server and client
    GUARDIAN_API_KEY: process.env.GUARDIAN_API_KEY,
    GUARDIAN_API_URL: process.env.GUARDIAN_API_URL,
    BASE_API_URL: process.env.BASE_API_URL,
  },
  images: {
    domains: ['media.guim.co.uk', 'localhost', 'showbiz365.net', 'api.showbiz365.net', 'api.giaitri365.net'],
  },
};

module.exports = config;
