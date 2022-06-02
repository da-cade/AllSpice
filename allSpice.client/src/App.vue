<template>
  <main>
    <Navbar />
    <div class="container-fluid full-page">
      <div class="row">
        <div class="col-12">
          <router-view />
        </div>
      </div>
    </div>
  </main>
</template>

<script>
import { computed } from 'vue'
import { AppState } from './AppState'
export default {
  name: 'App',
  setup() {
    return {
      appState: computed(() => AppState)
    }
  }
}
</script>
<style lang="scss">
@import "./assets/scss/main.scss";
.full-page {
  height: 100vh;
  flex-grow: 1;
}
body {
  background-color: #e9ecef;
}
.w-65 {
  width: 65%;
}
.w-35 {
  width: 35%;
}
.w-15 {
  width: 65%;
}
.w-85 {
  width: 65%;
}
</style>
